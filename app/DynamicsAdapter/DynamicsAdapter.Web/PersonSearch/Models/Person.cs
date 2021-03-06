﻿using Fams3Adapter.Dynamics.Identifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicsAdapter.Web.PersonSearch.Models
{
    // TODO: all classes bellow will be coming from SEARCH API CORE LIB
    public class PersonalIdentifierActual : PersonalIdentifier
    {
    }
    public class PersonalPhoneNumberActual : PersonalPhoneNumber
    { }

    public class PersonalAddressActual : PersonalAddress
    { }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IEnumerable<PersonalIdentifierActual> Identifiers { get; set; }

        public IEnumerable<PersonalPhoneNumberActual> PhoneNumbers { get; set; }
        public IEnumerable<PersonalAddressActual> Addresses { get; set; }

    }
}
