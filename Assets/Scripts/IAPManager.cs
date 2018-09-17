using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

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
        if (string.Equals(args.purchasedProduct.definition.id, currentProductID, System.StringComparison.Ordinal))
        {
            Debug.Log("SuccessBuy");
        }
        else
        {
            Debug.Log("FailBuy");
        }
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    #endregion 
}


