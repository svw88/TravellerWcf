﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using static TravelerWebService.Service1;

namespace TravelerWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        List<Events> GetEvents(string Country, string State, string City, string id);

        [OperationContract]
        List<Events> GetUserEvents(string alias, string id);

        [OperationContract]
        List<Events> SearchEvents(string Country, string State, string City, string id, string types, string find);

        [OperationContract]
        List<Events> GetMyEvents(string userId);

        [OperationContract]
        bool CreateEvent(Event post);

        [OperationContract]
        bool RemoveEvent(string eventId);

        [OperationContract]
        bool Register(User post);

        [OperationContract]
        string Login(string Email, string Password);

        [OperationContract]
        List<Countries> GetCountries();

        [OperationContract]
        List<States> GetStates(string name);

        [OperationContract]
        List<Cities> GetCities(string name);

        // TODO: Add your service operations here
    }
  
}
