﻿//
// Copyright © 2012 - 2013 Nauck IT KG     http://www.nauck-it.de
//
// Author:
//  Daniel Nauck        <d.nauck(at)nauck-it.de>
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Portable.Licensing.Validation;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Portable.Licensing.Tests
{
    public class LicenseValidationTests
    {
        public static IEnumerable<object[]> Can_Validate_Valid_Signature_Data()
        {
#if NET452
            yield return new object[]
            {
                @"MIIBKjCB4wYHKoZIzj0CATCB1wIBATAsBgcqhkjOPQEBAiEA/////wAAAAEAAAAAAAAAAAAAAAD///////////////8wWwQg/////wAAAAEAAAAAAAAAAAAAAAD///////////////wEIFrGNdiqOpPns+u9VXaYhrxlHQawzFOw9jvOPD4n0mBLAxUAxJ02CIbnBJNqZnjhE50mt4GffpAEIQNrF9Hy4SxCR/i85uVjpEDydwN9gS3rM6D0oTlF2JjClgIhAP////8AAAAA//////////+85vqtpxeehPO5ysL8YyVRAgEBA0IABNVLQ1xKY80BFMgGXec++Vw7n8vvNrq32PaHuBiYMm0PEj2JoB7qSSWhfgcjxNVJsxqJ6gDQVWgl0r7LH4dr0KU=",
                "MEUCIQCCEDAldOZHHIKvYZRDdzUP4V51y23d6deeK5jIFy27GQIgDz2CndjBh4Vb8tiC3FGQ6fn3GKt8d/P5+luJH0cWv+I=",
            };
#endif

            yield return new object[]
            {
                "MIGbMBAGByqGSM49AgEGBSuBBAAjA4GGAAQBMjD7TcXgSMbjDDpkOtNe68prK21mPv3c4q8+CSUZKSz9mO8YB0oXmXKCeKORp2v4bDhx6xqNsXCMX07GmgSm7n0A6q71AkjSGDz7iNrW2TSByFql38c6wdtCKneBu4R29u9z7VE/dfGuwDjmo0Fwpo4zaZSrubwCjqkMoU0fr/j7DtQ=",
                "MIGIAkIBK3XPiLibpWt64FffHsw+ypHl/4v1KUqa6jFjANQ0XKNREW9jJ3EUcspksz3fjeQbqtFackLkV20hKJZijHv95XUCQgHL9XTGEWhn0wHptDF0bW3AxRjpLyHjqlQ1FFw/d/9qKxSjN+gUMs+dHCMFGo7zwlRQpM3fQy6cQVDU72HLjTWzzg==",
            };
        }

        [Theory]
        [MemberData(nameof(Can_Validate_Valid_Signature_Data))]
        public void Can_Validate_Valid_Signature(string publicKey, string signature)
        {
            var licenseData = $@"<License>
                                  <Id>77d4c193-6088-4c64-9663-ed7398ae8c1a</Id>
                                  <Type>Trial</Type>
                                  <Expiration>Sun, 31 Dec 1899 23:00:00 GMT</Expiration>
                                  <Quantity>1</Quantity>
                                  <Customer>
                                    <Name>John Doe</Name>
                                    <Email>john@doe.tld</Email>
                                  </Customer>
                                  <LicenseAttributes />
                                  <ProductFeatures />
                                  <Signature>{signature}</Signature>
                                </License>";

            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .Signature(publicKey)
                .AssertValidLicense();

            Assert.NotNull(validationResults);
            Assert.Empty(validationResults);
        }

        public static IEnumerable<object[]> Can_Validate_Invalid_Signature_Data()
        {
#if NET452
            yield return new object[]
            {
                @"MIIBKjCB4wYHKoZIzj0CATCB1wIBATAsBgcqhkjOPQEBAiEA/////wAAAAEAAAAAAAAAAAAAAAD///////////////8wWwQg/////wAAAAEAAAAAAAAAAAAAAAD///////////////wEIFrGNdiqOpPns+u9VXaYhrxlHQawzFOw9jvOPD4n0mBLAxUAxJ02CIbnBJNqZnjhE50mt4GffpAEIQNrF9Hy4SxCR/i85uVjpEDydwN9gS3rM6D0oTlF2JjClgIhAP////8AAAAA//////////+85vqtpxeehPO5ysL8YyVRAgEBA0IABNVLQ1xKY80BFMgGXec++Vw7n8vvNrq32PaHuBiYMm0PEj2JoB7qSSWhfgcjxNVJsxqJ6gDQVWgl0r7LH4dr0KU=",
                "MEUCIQCCEDAldOZHHIKvYZRDdzUP4V51y23d6deeK5jIFy27GQIgDz2CndjBh4Vb8tiC3FGQ6fn3GKt8d/P5+luJH0cWv+I=",
            };
#endif

            yield return new object[]
            {
                "MIGbMBAGByqGSM49AgEGBSuBBAAjA4GGAAQBMjD7TcXgSMbjDDpkOtNe68prK21mPv3c4q8+CSUZKSz9mO8YB0oXmXKCeKORp2v4bDhx6xqNsXCMX07GmgSm7n0A6q71AkjSGDz7iNrW2TSByFql38c6wdtCKneBu4R29u9z7VE/dfGuwDjmo0Fwpo4zaZSrubwCjqkMoU0fr/j7DtQ=",
                "MIGIAkIBK3XPiLibpWt64FffHsw+ypHl/4v1KUqa6jFjANQ0XKNREW9jJ3EUcspksz3fjeQbqtFackLkV20hKJZijHv95XUCQgHL9XTGEWhn0wHptDF0bW3AxRjpLyHjqlQ1FFw/d/9qKxSjN+gUMs+dHCMFGo7zwlRQpM3fQy6cQVDU72HLjTWzzg==",
            };
        }

        [Theory]
        [MemberData(nameof(Can_Validate_Invalid_Signature_Data))]
        public void Can_Validate_Invalid_Signature(string publicKey, string signature)
        {
            var licenseData = $@"<License>
                                  <Id>77d4c193-6088-4c64-9663-ed7398ae8c1a</Id>
                                  <Type>Trial</Type>
                                  <Expiration>Sun, 31 Dec 1899 23:00:00 GMT</Expiration>
                                  <Quantity>999</Quantity>
                                  <Customer>
                                    <Name>John Doe</Name>
                                    <Email>john@doe.tld</Email>
                                  </Customer>
                                  <LicenseAttributes />
                                  <ProductFeatures />
                                  <Signature>{signature}</Signature>
                                </License>";

            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .Signature(publicKey)
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Single(validationResults);
            Assert.IsType<InvalidSignatureValidationFailure>(validationResults.FirstOrDefault());
        }

        [Fact]
        public void Can_Validate_Expired_ExpirationDate()
        {
            var licenseData = @"<License>
                                  <Id>77d4c193-6088-4c64-9663-ed7398ae8c1a</Id>
                                  <Type>Trial</Type>
                                  <Expiration>Sun, 31 Dec 1899 23:00:00 GMT</Expiration>
                                  <Quantity>1</Quantity>
                                  <Customer>
                                    <Name>John Doe</Name>
                                    <Email>john@doe.tld</Email>
                                  </Customer>
                                  <LicenseAttributes />
                                  <ProductFeatures />
                                  <Signature>MEUCIQCCEDAldOZHHIKvYZRDdzUP4V51y23d6deeK5jIFy27GQIgDz2CndjBh4Vb8tiC3FGQ6fn3GKt8d/P5+luJH0cWv+I=</Signature>
                                </License>";

            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .ExpirationDate()
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Single(validationResults);
            Assert.IsType<LicenseExpiredValidationFailure>(validationResults.FirstOrDefault());

        }

        public static IEnumerable<object[]> Can_Validate_CustomAssertion_Data()
        {
#if NET452
            yield return new object[]
            {
                @"MIIBKjCB4wYHKoZIzj0CATCB1wIBATAsBgcqhkjOPQEBAiEA/////wAAAAEAAAAAAAAAAAAAAAD///////////////8wWwQg/////wAAAAEAAAAAAAAAAAAAAAD///////////////wEIFrGNdiqOpPns+u9VXaYhrxlHQawzFOw9jvOPD4n0mBLAxUAxJ02CIbnBJNqZnjhE50mt4GffpAEIQNrF9Hy4SxCR/i85uVjpEDydwN9gS3rM6D0oTlF2JjClgIhAP////8AAAAA//////////+85vqtpxeehPO5ysL8YyVRAgEBA0IABNVLQ1xKY80BFMgGXec++Vw7n8vvNrq32PaHuBiYMm0PEj2JoB7qSSWhfgcjxNVJsxqJ6gDQVWgl0r7LH4dr0KU=",
                "MEUCIQCa6A7Cts5ex4rGHAPxiXpy+2ocZzTDSP7SsddopKUx5QIgHnqv0DjoOpc+K9wALqajxxvmLCRJAywCX5vDAjmWqr8=",
            };
#endif

            yield return new object[]
            {
                "MIGbMBAGByqGSM49AgEGBSuBBAAjA4GGAAQBMjD7TcXgSMbjDDpkOtNe68prK21mPv3c4q8+CSUZKSz9mO8YB0oXmXKCeKORp2v4bDhx6xqNsXCMX07GmgSm7n0A6q71AkjSGDz7iNrW2TSByFql38c6wdtCKneBu4R29u9z7VE/dfGuwDjmo0Fwpo4zaZSrubwCjqkMoU0fr/j7DtQ=",
                "MIGIAkIB9aL8HVou9zON76K02jeJCSaPXEPQ1oiBFzRD76kt9qUdZInotxAo1bJW0jODzdmKwxoPQESViwfdEJOQtfOj4PwCQgGMXU37vhPziaXkbGrkCXojYdpZt+s813Qi/ePlEVycyKjFrJVzhrxmIol36DqJWHie/uqzfBDHlQwWnzzrn7++FA==",
            };
        }

        [Theory]
        [MemberData(nameof(Can_Validate_CustomAssertion_Data))]
        public void Can_Validate_CustomAssertion(string publicKey, string signature)
        {
            var licenseData = $@"<License>
                              <Id>77d4c193-6088-4c64-9663-ed7398ae8c1a</Id>
                              <Type>Trial</Type>
                              <Expiration>Thu, 31 Dec 2009 23:00:00 GMT</Expiration>
                              <Quantity>1</Quantity>
                              <Customer>
                                <Name>John Doe</Name>
                                <Email>john@doe.tld</Email>
                              </Customer>
                              <LicenseAttributes>
                                <Attribute name=""Assembly Signature"">123456789</Attribute>
                              </LicenseAttributes>
                              <ProductFeatures>
                                <Feature name=""Sales Module"">yes</Feature>
                                <Feature name=""Workflow Module"">yes</Feature>
                                <Feature name=""Maximum Transactions"">10000</Feature>
                              </ProductFeatures>
                              <Signature>{signature}</Signature>
                            </License>";

            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .AssertThat(lic => lic.ProductFeatures.Contains("Sales Module"),
                            new GeneralValidationFailure { Message = "Sales Module not licensed!" })
                .And()
                .AssertThat(lic => lic.AdditionalAttributes.Get("Assembly Signature") == "123456789",
                            new GeneralValidationFailure { Message = "Assembly Signature does not match!" })
                .And()
                .Signature(publicKey)
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void Do_Not_Crash_On_Invalid_Data()
        {
            var publicKey = "1234";
            var licenseData =
                @"<license expiration='2013-06-30T00:00:00.0000000' type='Trial'><name>John Doe</name></license>";

            var license = License.Load(licenseData);

            var validationResults = license
                .Validate()
                .ExpirationDate()
                .And()
                .Signature(publicKey)
                .AssertValidLicense().ToList();

            Assert.NotNull(validationResults);
            Assert.Single(validationResults);
            Assert.IsType<InvalidSignatureValidationFailure>(validationResults.FirstOrDefault());

        }
    }
}