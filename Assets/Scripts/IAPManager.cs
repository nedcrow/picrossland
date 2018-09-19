using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class IAPManager : MonoBehaviour, IStoreListener {

    private static IStoreController m_StoreController; //Unity purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific purchasing subsystem.

    public static string currentProductID;
    public static string[] productIDCounsomable = { "cunsomable_gem_1", "cunsomable_gem_2", "cunsomable_gem_3" }; //
    public static string[] productNameGooglePlayComsumable = { "product_gem.1", "product_gem.2", "product_gem.3" };

    #region Reset

    void Start()
    {
        if(m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (Isinitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < productIDCounsomable.Length; i++)
        {
            builder.AddProduct(productIDCounsomable[i], ProductType.Consumable, new IDs()
            {
                {productNameGooglePlayComsumable[i], GooglePlay.Name },
            }); // input product
        }
        UnityPurchasing.Initialize(this, builder);
    } 


    private bool Isinitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }
    #endregion

    public void BuyConsumable(int i)
    {
        Debug.Log(productIDCounsomable[i]);
        currentProductID = productIDCounsomable[i];
        BuyProductID(productIDCounsomable[i]);
        
    }

    void BuyProductID(string productID)
    {
        if (Isinitialized())
        {
            Product product = m_StoreController.products.WithID(productID);

            if(product != null && product.availableToPurchase)
            {
                Debug.Log(product.definition.id);
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("Buy ProductID Fail : product null or product.availableToPurchasing()");
            }
        }
        else
        {
            Debug.Log("Buy ProductID Fail : Isinitialized()");
        }

    }

    #region IStoreListener
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason failureReason) 
    {
        Debug.Log(failureReason.ToString());
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        #region validate receipts
        bool validPurchase = true; // Presume valid for platforms with no R.V.

        // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
        // Prepare the validator with the secrets we prepared in the Editor
        // obfuscation window.
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
            AppleTangle.Data(), Application.identifier);

        try
        {
            var result = validator.Validate(args.purchasedProduct.receipt);
            Debug.Log("Receipt is valid. Contents:");
            foreach (IPurchaseReceipt productReceipt in result)
            {
                Debug.Log(productReceipt.productID);
                Debug.Log(productReceipt.purchaseDate);
                Debug.Log(productReceipt.transactionID);

                GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                if (null != google)
                {
                    // This is Google's Order ID.
                    // Note that it is null when testing in the sandbox
                    // because Google's sandbox does not provide Order IDs.
                    Debug.Log(google.transactionID);
                    Debug.Log(google.purchaseState);
                    Debug.Log(google.purchaseToken);
                }

                //AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                //if (null != apple)
                //{
                //    Debug.Log(apple.originalTransactionIdentifier);
                //    Debug.Log(apple.subscriptionExpirationDate);
                //    Debug.Log(apple.cancellationDate);
                //    Debug.Log(apple.quantity);
                //}
            }
        }
        catch (IAPSecurityException)
        {
            Debug.Log("Invalid receipt, not unlocking content");
            validPurchase = false;
        }
#endif

        if (validPurchase)
        {
            string[] wards = currentProductID.Split('_');
            int num = System.Convert.ToInt32(wards[wards.Length - 1]);
            switch (num)
            {
                case 1:
                    num = 33;
                    break;
                case 2:
                    num = 111;
                    break;
                case 3:
                    num = 333;
                    break;
                default:
                    Debug.Log("ProductID ERROR : " + wards);
                    break;
            }
            UserManager.Instance.currentUser.gem += num;
            LandManager.instance.views.againView.transform.GetChild(0).GetComponent<AdminViewController>().SetAdminView();
        }        //유효한 구매
        #endregion

        if (string.Equals(args.purchasedProduct.definition.id, currentProductID, System.StringComparison.Ordinal)) //주소값으로 비교
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            //Debug.Log("SuccessBuy");
        }
        else
        {
            Debug.Log("FailBuy");
        }
        return PurchaseProcessingResult.Complete;
    }//googleplay 영수증 인증.

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    #endregion 
}


