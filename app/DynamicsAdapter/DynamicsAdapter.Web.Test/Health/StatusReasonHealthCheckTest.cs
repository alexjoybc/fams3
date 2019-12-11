﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicsAdapter.Web.Health;
using DynamicsAdapter.Web.SearchRequest;
using DynamicsAdapter.Web.Test.FakeMessages;
using Fams3Adapter.Dynamics.OptionSets;
using Fams3Adapter.Dynamics.OptionSets.Models;
using Fams3Adapter.Dynamics.Types;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Moq;
using NUnit.Framework;


namespace DynamicsAdapter.Web.Test.Health
{
    public class StatusReasonHealthCheckTest
    {
        private StatusReasonHealthCheck _sut;
        private readonly Mock<IOptionSetService> _statusReasonServiceMock = new Mock<IOptionSetService>();

        [Test]
        public async Task with_different_statuses_should_return_a_collection_of_search_request()
        {

            _statusReasonServiceMock.Setup(x => x.GetAllStatusCode(It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult(FakeHttpMessageResponse.GetFakeInvalidReason()));
            _sut = new StatusReasonHealthCheck(_statusReasonServiceMock.Object);

            var result = await _sut.CheckHealthAsync(new HealthCheckContext() ,CancellationToken.None);
            Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        }

        [Test]
        public async Task with_same_statuses_different_options_set_should_return_unhealthy()
        {

            _statusReasonServiceMock.Setup(x => x.GetAllStatusCode(It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult(FakeHttpMessageResponse.GetFakeValidReason()));

            var fakeIdentificationTypes = Enumeration.GetAll<IdentificationType>().ToList();
            fakeIdentificationTypes.RemoveAt(0);
            
            _statusReasonServiceMock.Setup(x => x.GetAllOptions("ssgidentificationtypes", CancellationToken.None))
                .Returns(Task.FromResult(fakeIdentificationTypes.AsEnumerable().Select(x => new GenericOption(x.Value, x.Name))));
            _sut = new StatusReasonHealthCheck(_statusReasonServiceMock.Object);

            var result = await _sut.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);
            Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        }


        [Test]
        public async Task with_same_statuses_same_options_set_should_return_healthy()
        {

            _statusReasonServiceMock.Setup(x => x.GetAllStatusCode(It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult(FakeHttpMessageResponse.GetFakeValidReason()));
            _statusReasonServiceMock.Setup(x => x.GetAllOptions("ssg_identificationtypes", CancellationToken.None))
                .Returns(Task.FromResult(Enumeration.GetAll<IdentificationType>().Select(x => new GenericOption(x.Value, x.Name))));

            _statusReasonServiceMock.Setup(x => x.GetAllOptions("ssg_canadianprovincecodesimpletype", CancellationToken.None))
      .Returns(Task.FromResult(Enumeration.GetAll<CanadianProvinceType>().Select(x => new GenericOption(x.Value, x.Name))));

            _statusReasonServiceMock.Setup(x => x.GetAllOptions("ssg_informationsourcecodes", CancellationToken.None))
      .Returns(Task.FromResult(Enumeration.GetAll<InformationSourceType>().Select(x => new GenericOption(x.Value, x.Name))));

            _statusReasonServiceMock.Setup(x => x.GetAllOptions("ssg_addresscategorycodes", CancellationToken.None))
      .Returns(Task.FromResult(Enumeration.GetAll<LocationType>().Select(x => new GenericOption(x.Value, x.Name))));

            _statusReasonServiceMock.Setup(x => x.GetAllOptions("ssg_telephonenumbercategorycodes", CancellationToken.None))
      .Returns(Task.FromResult(Enumeration.GetAll<TelephoneNumberType>().Select(x => new GenericOption(x.Value, x.Name))));

            _sut = new StatusReasonHealthCheck(_statusReasonServiceMock.Object);

            var result = await _sut.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);
            Assert.AreEqual(HealthStatus.Healthy, result.Status);
        }


        [Test]
        public async Task with_empty_statuses_should_return_unhealthy()
        {

            _statusReasonServiceMock.Setup(x => x.GetAllStatusCode(It.IsAny<string>(), CancellationToken.None))
                .Returns(Task.FromResult(FakeHttpMessageResponse.GetFakeNullResult()));
            _sut = new StatusReasonHealthCheck(_statusReasonServiceMock.Object);

            var result = await _sut.CheckHealthAsync(new HealthCheckContext(), CancellationToken.None);
            Assert.AreEqual(HealthStatus.Unhealthy, result.Status);
        }
    }
}
