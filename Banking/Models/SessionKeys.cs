using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using Banking.ViewModels;
using BankingLib.Models;

namespace Banking.Models
{
    public class SessionKeyBase
    {
        public static T GetFromSession<T>(ISession session, string key)
        {
            string json = session.GetString(key);
            if (string.IsNullOrEmpty(json))
                return default(T);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void SetToSession<T>(ISession session, string key, T model)
        {
            string json = JsonConvert.SerializeObject(model);
            session.SetString(key, json);
        }
    }

    public class BillPaySessionKey : SessionKeyBase
    {
        public static string EditOrCreate = "BillPayEditOrCreateViewModel";
        public static string EditBillPayID = "BillPayEditBillPayID";

        public static BillPayEditViewModel GetEditViewModelFromSession(ISession session)
        {
            return GetFromSession<BillPayEditViewModel>(session, EditOrCreate);
        }

        public static void SetEditViewModelToSession(BillPayEditViewModel model, ISession session)
        {
            SetToSession(session, EditOrCreate, model);
        }

        public static void Clear(ISession session) 
        {
            session.Remove(EditOrCreate);
            session.Remove(EditBillPayID);
        }

    }

    public class CustomerSessionKey : SessionKeyBase
    {
        public static string CustomerIDK = "LoggedinCustomerID";
        public static string CustomerNameK = "LoggedinCustomerName";
        public static void SetToSession(Customer customer, ISession session) 
        {
            session.SetInt32(CustomerIDK, customer.CustomerID);
            session.SetString(CustomerNameK, customer.Name);
        }

        public static int? GetCustomerID(ISession session)
        {
            return session.GetInt32(CustomerIDK);
        }

        public static string GetName(ISession session) 
        {
            return session.GetString(CustomerNameK);
        }
    }
}
