namespace SaatchiDataCapture.Logic.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using SaatchiDataCapture.Data.Definitions;
    using SaatchiDataCapture.Data.Models;
    using SaatchiDataCapture.Logic.Definitions;
    using SaatchiDataCapture.Models;

    [TestClass]
    public class PersonManagerTests
    {
        [TestMethod]
        public void Create_EmailAddressExistsAlready_ThrowsPersonRecordExistsAlreadyException()
        {
            // Arrange
            ReadPersonResult readPersonResult = new ReadPersonResult()
            {
                Id = 1234,
                ContactDetail_Id = 9876,
            };

            PersonManager personManager = this.CreatePersonManagerInstance(
                mockDataCaptureDatabaseAdapter =>
                {
                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.ReadPerson(
                            It.IsAny<string>()))
                        .Returns(readPersonResult);
                });

            Person person = new Person()
            {
                Consent = new Consent()
                {
                    GdprConsentDeclared = DateTime.Parse("2018-11-19 08:30:21"),
                    GdprConsentGiven = true,
                },
                Cookie = new Cookie()
                {
                    Captured = DateTime.Parse("2018-11-19 08:31:11"),
                    CookieIdentifier = "ABC0123456789",
                },
                Route = new Route()
                {
                    Captured = DateTime.Parse("2018-11-19 08:29:03"),
                    RouteIdentifier = "ZYX9876543210",
                },
                ContactDetail = new ContactDetail()
                {
                    Captured = DateTime.Parse("2018-11-18 08:30:21"),
                    EmailAddress = "joe.bloggs@somecorp.local",
                    EmailVerificationCompletion = DateTime.Parse("2018-11-18 13:00:23"),
                },
                Enrolled = DateTime.Parse("2018-11-18 08:30:20"),
                FirstName = "Joe",
                LastName = "Bloggs",
            };

            PersonRecordExistsAlreadyException thrownException = null;

            // Act
            try
            {
                personManager.Create(person);
            }
            catch (PersonRecordExistsAlreadyException personRecordExistsAlreadyException)
            {
                thrownException = personRecordExistsAlreadyException;
            }

            // Assert
            Assert.IsNotNull(thrownException);
        }

        [TestMethod]
        public void Create_EmailAddressDoesNotExistAlready_NoExceptionThrown()
        {
            // Arrange
            ReadPersonResult readPersonResult = null;

            CreatePersonResult createPersonResult = new CreatePersonResult()
            {
                Id = 123,
            };

            CreateContactDetailResult createContactDetailResult =
                new CreateContactDetailResult()
                {
                    Id = 456,
                };

            CreateConsentResult createConsentResult = new CreateConsentResult()
            {
                Id = 789,
            };

            CreateCookieResult createCookieResult = new CreateCookieResult()
            {
                Id = 345,
            };

            CreateRouteResult createRouteResult = new CreateRouteResult()
            {
                Id = 678,
            };

            PersonManager personManager = this.CreatePersonManagerInstance(
                mockDataCaptureDatabaseAdapter =>
                {
                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.ReadPerson(
                            It.IsAny<string>()))
                        .Returns(readPersonResult);

                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.CreatePerson(
                            It.IsAny<DateTime>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<string>(),
                            It.IsAny<string>()))
                        .Returns(createPersonResult);

                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.CreateContactDetail(
                            It.IsAny<long>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<string>(),
                            It.IsAny<DateTime?>()))
                        .Returns(createContactDetailResult);

                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.CreateConsent(
                            It.IsAny<long>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<bool?>()))
                        .Returns(createConsentResult);

                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.CreateCookie(
                            It.IsAny<long>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<string>()))
                        .Returns(createCookieResult);

                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.CreateRoute(
                            It.IsAny<long>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<DateTime>(),
                            It.IsAny<string>()))
                        .Returns(createRouteResult);
                });

            Person person = new Person()
            {
                Consent = new Consent()
                {
                    GdprConsentDeclared = DateTime.Parse("2018-11-19 08:30:21"),
                    GdprConsentGiven = true,
                },
                Cookie = new Cookie()
                {
                    Captured = DateTime.Parse("2018-11-19 08:31:11"),
                    CookieIdentifier = "ABC0123456789",
                },
                Route = new Route()
                {
                    Captured = DateTime.Parse("2018-11-19 08:29:03"),
                    RouteIdentifier = "ZYX9876543210",
                },
                ContactDetail = new ContactDetail()
                {
                    Captured = DateTime.Parse("2018-11-18 08:30:21"),
                    EmailAddress = "joe.bloggs@somecorp.local",
                    EmailVerificationCompletion = DateTime.Parse("2018-11-18 13:00:23"),
                },
                Enrolled = DateTime.Parse("2018-11-18 08:30:20"),
                FirstName = "Joe",
                LastName = "Bloggs",
            };

            PersonRecordExistsAlreadyException thrownException = null;

            // Act
            try
            {
                personManager.Create(person);
            }
            catch (PersonRecordExistsAlreadyException personRecordExistsAlreadyException)
            {
                thrownException = personRecordExistsAlreadyException;
            }

            // Assert
            Assert.IsNull(thrownException);
        }

        [TestMethod]
        public void Update_EmailAddressDoesNotExistAlready_ThrowsException()
        {
            // Arrange
            ReadPersonResult readPersonResult = null;
            PersonManager personManager = this.CreatePersonManagerInstance(
                mockDataCaptureDatabaseAdapter =>
                {
                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.ReadPerson(It.IsAny<string>()))
                        .Returns(readPersonResult);
                });

            Person person = new Person()
            {
                Consent = new Consent()
                {
                    GdprConsentDeclared = DateTime.Parse("2018-11-19 08:30:21"),
                    GdprConsentGiven = true,
                },
                Cookie = new Cookie()
                {
                    Captured = DateTime.Parse("2018-11-19 08:31:11"),
                    CookieIdentifier = "ABC0123456789",
                },
                Route = new Route()
                {
                    Captured = DateTime.Parse("2018-11-19 08:29:03"),
                    RouteIdentifier = "ZYX9876543210",
                },
                ContactDetail = new ContactDetail()
                {
                    Captured = DateTime.Parse("2018-11-18 08:30:21"),
                    EmailAddress = "joe.bloggs@somecorp.local",
                    EmailVerificationCompletion = DateTime.Parse("2018-11-18 13:00:23"),
                },
                Enrolled = DateTime.Parse("2018-11-18 08:30:20"),
                FirstName = "Joe",
                LastName = "Bloggs",
            };

            PersonRecordDoesNotExistException thrownException = null;

            // Act
            try
            {
                personManager.Update(person, true);
            }
            catch (PersonRecordDoesNotExistException personRecordDoesNotExistException)
            {
                thrownException = personRecordDoesNotExistException;
            }

            // Assert
            Assert.IsNotNull(thrownException);
        }

        [TestMethod]
        public void Update_EmailAddressExistsAlready_NoExceptionThrown()
        {
            // Arrange
            ReadPersonResult readPersonResult = new ReadPersonResult()
            {
                Id = 1234,
                ContactDetail_Id = 9876,
            };

            PersonManager personManager = this.CreatePersonManagerInstance(
                mockDataCaptureDatabaseAdapter =>
                {
                    mockDataCaptureDatabaseAdapter
                        .Setup(x => x.ReadPerson(It.IsAny<string>()))
                        .Returns(readPersonResult);
                });

            Person person = new Person()
            {
                Consent = new Consent()
                {
                    GdprConsentDeclared = DateTime.Parse("2018-11-19 08:30:21"),
                    GdprConsentGiven = true,
                },
                Cookie = new Cookie()
                {
                    Captured = DateTime.Parse("2018-11-19 08:31:11"),
                    CookieIdentifier = "ABC0123456789",
                },
                Route = new Route()
                {
                    Captured = DateTime.Parse("2018-11-19 08:29:03"),
                    RouteIdentifier = "ZYX9876543210",
                },
                ContactDetail = new ContactDetail()
                {
                    Captured = DateTime.Parse("2018-11-18 08:30:21"),
                    EmailAddress = "joe.bloggs@somecorp.local",
                    EmailVerificationCompletion = DateTime.Parse("2018-11-18 13:00:23"),
                },
                Enrolled = DateTime.Parse("2018-11-18 08:30:20"),
                FirstName = "Joe",
                LastName = "Bloggs",
            };

            PersonRecordDoesNotExistException thrownException = null;

            // Act
            try
            {
                personManager.Update(person, true);
            }
            catch (PersonRecordDoesNotExistException personRecordDoesNotExistException)
            {
                thrownException = personRecordDoesNotExistException;
            }

            // Assert
            Assert.IsNull(thrownException);
        }

        private PersonManager CreatePersonManagerInstance(
            Action<Mock<IDataCaptureDatabaseAdapter>> mockDataCaptureDatabaseAdapterSetup)
        {
            PersonManager toReturn = null;

            Mock<IDataCaptureDatabaseAdapter> mockDataCaptureDatabaseAdapter =
                new Mock<IDataCaptureDatabaseAdapter>();
            Mock<ILoggerProvider> mockLoggerProvider =
                new Mock<ILoggerProvider>();

            mockDataCaptureDatabaseAdapterSetup(
                mockDataCaptureDatabaseAdapter);

            toReturn = new PersonManager(
                mockDataCaptureDatabaseAdapter.Object,
                mockLoggerProvider.Object);

            return toReturn;
        }
    }
}