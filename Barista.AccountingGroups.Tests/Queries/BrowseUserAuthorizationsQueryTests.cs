﻿using Barista.AccountingGroups.Domain;
using Barista.AccountingGroups.Queries;
using Barista.Common.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using AutoMapper;
using Barista.AccountingGroups.Dto;
using Moq;

namespace Barista.AccountingGroups.Tests.Queries
{
    [TestClass]
    public class BrowseUserAuthorizationsQueryTests : BrowseQueryTestBase<BrowseUserAuthorizations, UserAuthorization>
    {
        protected override QueryOption[] Options => new[]
        {
            new QueryOption
            {
                Name = nameof(BrowseUserAuthorizations.AccountingGroupId),
                QueryConfiguratorAction = q => q.AccountingGroupId = TestIds.B,
                SampleData = new []
                {
                    new UserAuthorization(TestIds.A, Guid.Empty, UserAuthorizationLevel.AuthorizedUser),
                    new UserAuthorization(TestIds.B, Guid.Empty, UserAuthorizationLevel.AuthorizedUser)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.B, data.Single().AccountingGroupId)
            },

            new QueryOption
            {
                Name = nameof(BrowseUserAuthorizations.UserId),
                QueryConfiguratorAction = q => q.UserId = TestIds.B,
                SampleData = new []
                {
                    new UserAuthorization(TestIds.C, TestIds.A, UserAuthorizationLevel.AuthorizedUser),
                    new UserAuthorization(TestIds.D, TestIds.B, UserAuthorizationLevel.AuthorizedUser)
                },
                SampleDataValidator = data => Assert.AreEqual(TestIds.D, data.Single().AccountingGroupId)
            },

            new QueryOption
            {
                Name = nameof(BrowseUserAuthorizations.UserAuthorizationLevel),
                QueryConfiguratorAction = q => q.UserAuthorizationLevel = UserAuthorizationLevelDto.Owner,
                SampleData = new []
                {
                    new UserAuthorization(Guid.Empty, Guid.Empty, UserAuthorizationLevel.AuthorizedUser),
                    new UserAuthorization(Guid.Empty, Guid.Empty, UserAuthorizationLevel.Owner),                    
                },
                SampleDataValidator = data => Assert.AreEqual(UserAuthorizationLevel.Owner, data.Single().Level)
            }
        };
        
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>(MockBehavior.Strict);

        public BrowseUserAuthorizationsQueryTests()
        {
            _mapperMock.Setup(m => m.Map<UserAuthorizationLevel>(It.IsAny<UserAuthorizationLevelDto>()))
                .Returns<UserAuthorizationLevelDto>((dto) => Enum.Parse<UserAuthorizationLevel>(dto.ToString()));
        }

        protected override BrowseUserAuthorizations InstantiateQuery() => new BrowseUserAuthorizations();
    }
}
