using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using WarehouseApp.Data.Context;
using WarehouseApp.Data.Models;

namespace WarehouseApp.Data.Services
{
    public interface IServiceOrgs
    {
        public ApplicationContext GetContext();
        public ObservableCollection<Organization> GetAll();
        public bool AddOrg(Organization org);
        public bool EditOrg(Organization org);
        public bool DelOrg(Organization org);
    }


    public class ServiceOrgs : IServiceOrgs
    {
        private ApplicationContext _context {  get; set; }

        public ServiceOrgs(ApplicationContext context)
        {
            this._context = context;
        }

        public ObservableCollection<Organization> GetAll()
        {
            return this._context.Orgs;
        }

        public bool AddOrg(Organization org)
        {
            bool result = false;
            if (org != null)
            {
                if (!string.IsNullOrEmpty(org.OrgName))
                {
                    if (!this._context.Orgs.Contains(org))
                    {
                        this._context.Orgs.Add(org);
                        result = true;
                    }
                }
            }

            return result;
        }

        public bool DelOrg(Organization org)
        {
            bool result = false;
            if (org != null)
            {
                if (!string.IsNullOrEmpty(org.OrgName))
                {
                    if (this._context.Orgs.Contains(org))
                    {
                        this._context.Orgs.Remove(org);
                        result = true;
                    }
                }
            }

            return result;
        }

        public bool EditOrg(Organization org)
        {
            bool result = false;
            if (org != null)
            {
                if (!string.IsNullOrEmpty(org.OrgName))
                {
                    Organization? target = this._context.Orgs
                        .FirstOrDefault(o => o.Id == org.Id);

                    if (target != null)
                    {
                        int id = this._context.Orgs.IndexOf(target);
                        if (id != -1)
                        {
                            this._context.Orgs[id] = org;
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
    }
}
