using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class NewMyIAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController m_StoreController; // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    private IAppleExtensions m_AppleExtensions;
    private IGooglePlayStoreExtensions m_GoogleExtensions;

    private static NewMyIAPManager instance;

    public static NewMyIAPManager Instance => instance;

    void OnEnable()
    {
        Debug.Log("Enable NewMyIAPManager");
        instance = this;
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing, can use button click instead
            Debug.Log("Enable NewMyIAPManager InitializePurchasing");
            InitializePurchasing();
        }
    }

    [ContextMenu("Init")]
    public void MyInitialize()
    {
        InitializePurchasing();
    }

    private void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (var item in GlobalDataManager.Ins.storeData.productInfos.Where(item => item.typePrice != TypePrice.Ads))
        {
            builder.AddProduct(item.id, item.productType);
        }

        Debug.Log("NewMyIAPManager Starting Initialized...");
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        Debug.Log("NewMyIAPManager m_StoreController is null: ? " + (m_StoreController != null));
        Debug.Log("NewMyIAPManager m_StoreExtensionProvider is null: ? " + (m_StoreExtensionProvider != null));
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void RestorePurchases()
    {
        m_StoreExtensionProvider.GetExtension<IAppleExtensions>().RestoreTransactions(result =>
        {
            Debug.Log(result ? "Restore purchases succeeded." : "Restore purchases failed.");
        });
    }

    public void BuyProductByPayItem()
    {
        Debug.Log("NewMyIAPManager BuyProductIDByIndex IsInitialized:" + IsInitialized());
        // var productId = IAPDataSO.Instance.GetProductId(item);
        // if (string.IsNullOrEmpty(productId))
        // {
        //     Debug.Log("Không tồn tại sản phẩm!");
        // }
        // else
        // {
        //     BuyProductID(IAPDataSO.Instance.GetProductId(item));
        // }
    }

    public void BuyProductID(string productId)
    {
        // if (IAPDataSO.Instance.IsTestMode)
        // {
        //     IAPDataSO.Instance.BuyTestWallet(productId);
        //     return;
        // }
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                // có thể đã purchase với id productId
                Debug.Log(string.Format("Purchasing product:" + product.definition.id.ToString()));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log(
                    "NewMyIAPManager BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("NewMyIAPManager BuyProductID FAIL. Not initialized.");
            InitializePurchasing();
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("NewMyIAPManager OnInitialized: PASS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        m_GoogleExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("NewMyIAPManager OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log($"OnInitializeFailed InitializationFailureReason: {error}, message: {message}");
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        // đã nỗ lực purchase với id productId
        Debug.Log("ProcessPurchase");
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
        bool validPurchase = true;
        try
        {
            var result = validator.Validate(args.purchasedProduct.receipt);

            Debug.Log("Validate = " + result.ToString());

            foreach (IPurchaseReceipt productReceipt in result)
            {
                Debug.Log($"productReceipt.productID:{productReceipt.productID}");
                Debug.Log($"productReceipt.purchaseDate:{productReceipt.purchaseDate}");
                Debug.Log($"productReceipt.transactionID:{productReceipt.transactionID}");
            }
        }
        catch (Exception e)
        {
            Debug.Log("Invalid receipt, error is " + e.Message.ToString());
            validPurchase = false;
        }

        Debug.Log(string.Format("ProcessPurchase: " + args.purchasedProduct.definition.id));

        if (validPurchase)
        {
            Debug.Log("Valid receipt");
            // onPurchaseComplete.Invoke(args.purchasedProduct);
            foreach (IPurchaseReceipt productReceipt in validator.Validate(args.purchasedProduct.receipt))
            {
                // Unlock the appropriate content here.
#if UNITY_ANDROID
                GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                if (google != null)
                {
                    Debug.Log("full receipt json info: " + args.purchasedProduct.receipt);
                    //Get and parse the data you need to upload. Parse into string type
                    var wrapper =
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(args.purchasedProduct.receipt);
                    Debug.Log("convert wrapper successfully");
                    if (null == wrapper)
                    {
                        // return (consumePurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;
                        return PurchaseProcessingResult.Pending;
                    }


                    var payload =
                        (string)wrapper["Payload"]; // For Apple this will be the base64 encoded ASN.1 receipt
                    //For GooglePlay payload contains more JSON 
                    Debug.Log("payload" + payload);
                    var gpDetails = JsonConvert.DeserializeObject<Dictionary<string, object>>(payload);
                    var gpSig = (string)gpDetails["signature"];

                    var googleWalletInfo = new GoogleWalletInfo
                    {
                        productId = args.purchasedProduct.definition.id,
                        signature = gpSig,
                        orderId = google.orderID,
                        packageName = google.packageName,
                        //purchaseTime = DateTimeHelper.GetTimeStamp(google.purchaseDate),
                        purchaseState = GetPurchaseStateInt(google.purchaseState),
                        purchaseToken = google.purchaseToken,
                        payPercentId = 0
                    };

                    //GlobalData.Ins.storeData.OnPayWalletSuccess(args.purchasedProduct.definition.id);
                }
#endif

#if UNITY_IOS
                // AppleReceipt apple = productReceipt as AppleReceipt;
                AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                if (apple != null)
                {
                    Debug.Log("full receipt json info");
                    Debug.Log(args.purchasedProduct.receipt);
                    string transactionReceipt =
 m_AppleExtensions.GetTransactionReceiptForProduct(args.purchasedProduct);


                    var appleWalletInfo =
 new AppleWalletInfo(transactionReceipt);
                    GlobalData.Ins.storeData.OnPayWalletSuccess(args.purchasedProduct.definition.id);
                }
#endif
            }
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason));
    }

    private int GetPurchaseStateInt(GooglePurchaseState state)
    {
        switch (state)
        {
            case GooglePurchaseState.Purchased: return 0;
            default: return 1;
        }
    }
}