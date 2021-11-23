using System;
using NUnit.Framework;

namespace LinqToYourDoom.Tests.System.Extensions;

static class LazyExtensionsTests {
	[Test]
	public static void As_To_Do_Lazy() {
		var callCount = 0;
		var upperCase = string.Empty;

		var source = new Lazy<object>(() => {
			++callCount;
			return "abcd";
		});

		var derived = source
			.As<string>()
			.Into(it => {
				++callCount;
				upperCase = it.ToUpperInvariant();
			})
			.To(it => {
				++callCount;
				return it.Length;
			});

		Assert.AreEqual(0, callCount);

		_ = source.Value;

		Assert.AreEqual(1, callCount);

		var length = derived.Value;

		Assert.AreEqual(3, callCount);
		Assert.AreEqual(4, length);
	}
}
