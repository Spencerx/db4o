/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2011  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */
using Db4oTool.Tests.Core;
using Db4oUnit;

class MyCustomAttribute : System.Attribute
{
}

[MyCustomAttribute]
class Instrumented
{
	public static void Foo()
	{
	}
}

class NotInstrumented
{
	public static void Bar()
	{
	}
}

class ByAttributeInstrumentationSubject : ITestCase
{
	private static void RunFooBar()
	{
		Instrumented.Foo();
		NotInstrumented.Bar();
	}

	public void Test()
	{
		string stdout = ShellUtilities.WithStdout(RunFooBar);

		string expected =
			@"
TRACE: System.Void Instrumented::Foo()
";
		Assert.AreEqual(expected.Trim(), stdout);
	}
}
