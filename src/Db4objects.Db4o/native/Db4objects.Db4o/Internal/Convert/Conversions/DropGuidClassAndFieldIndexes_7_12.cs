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
using System;
using Db4objects.Db4o.Reflect.Net;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
	public partial class DropGuidClassAndFieldIndexes_7_12 : DropClassIndexesConversion
	{
		private readonly FieldReindexer<Guid> reindexer = new FieldReindexer<Guid>();

		protected override bool Accept(ClassMetadata classmetadata)
		{
			var isGuid = NetReflector.ToNative(classmetadata.ClassReflector()) == typeof (Guid);
			if (!isGuid)
			{
				classmetadata.TraverseDeclaredFields(reindexer);
			}
			return isGuid;
		}
	}
}
