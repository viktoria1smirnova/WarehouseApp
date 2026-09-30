using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WarehouseApp.Data.Context;
using WarehouseApp.Data.Models;

namespace WarehouseApp.Data.Services
{
    public interface IServiceWhs
    {
        public ApplicationContext GetContext();
        public ObservableCollection<Warehouse> GetAll();
        public bool AddWhs(Warehouse whs);
        public bool EditWhs(Warehouse whs);
        public bool DelWhs(Warehouse whs);
    }

    public class ServiceWhs : IServiceWhs
    {
        private ApplicationContext _context;
        private Organization _org;

        public ServiceWhs(ApplicationContext context, Organization org)
        {
            if (context != null && org != null)
            {
                this._context = context;
                if (_context.Orgs.Contains(org))
                    this._org = org;
            }
        }

        public ObservableCollection<Warehouse> GetAll()
        {
            return new ObservableCollection<Warehouse>(this._context.Whs
                .Where(x => x.OrgId == this._org.Id)
                .ToList());
        }

        public bool AddWhs(Warehouse whs)
        {
            bool result = false;
            if (whs != null)
            {
                if (!string.IsNullOrEmpty(whs.WhName))
                {
                    if (!this._context.Whs.Contains(whs))
                    {
                        this._context.Whs.Add(whs);
                        result = true;
                    }
                }
            }

            return result;
        }

        public bool DelWhs(Warehouse whs)
        {
            bool result = false;
            if (whs != null)
            {
                if (!string.IsNullOrEmpty(whs.WhName))
                {
                    Warehouse? target = this._context.Whs
                        .FirstOrDefault(w => w.Id == whs.Id);

                    if (target != null)
                    {
                        this._context.Whs.Remove(target);
                        result = true;
                    }
                }
            }

            return result;
        }

        public bool EditWhs(Warehouse whs)
        {
            bool result = false;
            if (whs != null)
            {
                if (!string.IsNullOrEmpty(whs.WhName))
                {
                    Warehouse? target = this._context.Whs
                        .FirstOrDefault(w => w.Id == whs.Id);

                    if (target != null)
                    {
                        int idx = this._context.Whs.IndexOf(target);
                        if (idx != -1)
                        {
                            this._context.Whs[idx] = whs;
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        public ApplicationContext GetContext()
        {
            return this._context;
        }

        public Organization GetOrganization()
        {
            return this._org;
        }
    }
}
