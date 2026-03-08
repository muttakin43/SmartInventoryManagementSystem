using SmartInventory.DAL.Content;
using SmartInventory.DAL.Core;
using SmartInventory.DAL.Interface;


namespace SmartInventory.BLL.Implementation
{
    public class PurchaseUnitOfWork : UnitofWork, IPurchaseUnitOfWork
    {
       

        public PurchaseUnitOfWork(
            SmartInventoryDbContext context,
            IPurchaseRepository purchaseRepository,
            IPurchaseDetailsRepository purchaseDetailsRepository,
            IProductRepository productRepository
            ) : base(context)
        {
            PurchaseRepository = purchaseRepository;
            PurchaseDetailsRepository = purchaseDetailsRepository;
            ProductRepository = productRepository;
        }

        public IPurchaseRepository PurchaseRepository { get; }

        public IPurchaseDetailsRepository PurchaseDetailsRepository { get; }

        public IProductRepository ProductRepository { get; }


    }
}