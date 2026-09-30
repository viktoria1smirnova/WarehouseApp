using System.Collections.ObjectModel;
using WarehouseApp.Data.Context;
using WarehouseApp.Data.Models;

namespace WarehouseApp.Data.Services
{
    public interface IServiceInvoices
    {
        ApplicationContext GetContext();
        ObservableCollection<Invoice> GetAll();
        bool AddInvoice(Invoice invoice);
        bool DelInvoice(Invoice invoice);
    }

    public class ServiceInvoices : IServiceInvoices
    {
        private ApplicationContext _context;
        private Warehouse _whs;

        public ServiceInvoices(ApplicationContext context, Warehouse whs)
        {
            _context = context;
            this._whs = whs;
        }

        public ObservableCollection<Invoice> GetAll()
        {
            return new ObservableCollection<Invoice>(this._context.Invcs
               .Where(x => x.WhsId == this._whs.Id)
               .ToList());
        }

        public bool AddInvoice(Invoice invoice)
        {
            bool result = false;

            if (invoice != null && invoice.Items.Count > 0)
            {
                this._context.Invcs.Add(invoice);
                result = true;
            }

            return result;
        }

        public bool DelInvoice(Invoice invoice)
        {
            bool result = false;
            if (invoice != null)
            {
                Invoice? target = this._context.Invcs
                    .FirstOrDefault(i => i.Id == invoice.Id);

                if (target != null)
                {
                    this._context.Invcs.Remove(target);
                    result = true;
                }
            }
            return result;
        }

        public ApplicationContext GetContext()
        {
            return this._context;
        }

        public Warehouse GetWarehouse()
        {
            return this._whs;
        }
    }
}
