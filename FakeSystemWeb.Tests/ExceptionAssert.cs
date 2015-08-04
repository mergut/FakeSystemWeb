/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2015 Mehmet Ergut
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
namespace FakeSystemWeb.Tests
{
    using System;
    using NUnit.Framework;

    public static class ExceptionAssert
    {
        public static void ArgumentNull(TestDelegate code, string paramName)
        {
            Assert.That(code, Throws.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo(paramName));
        }

        public static void Argument(TestDelegate code, string message, string paramName)
        {
            var constraint =
                Throws.ArgumentException
                    .With.Message.StringStarting(message)
                    .And
                    .With.Property("ParamName").EqualTo(paramName);

            Assert.That(code, constraint);
        }
    }
}
