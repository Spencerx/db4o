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
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Internal.Btree
{
	/// <exclude></exclude>
	public class BTreeConfiguration
	{
		public static readonly Db4objects.Db4o.Internal.Btree.BTreeConfiguration Default = 
			new Db4objects.Db4o.Internal.Btree.BTreeConfiguration(null, 20, true);

		public readonly ITransactionalIdSystem _idSystem;

		public readonly SlotChangeFactory _slotChangeFactory;

		public readonly bool _canEnlistWithTransaction;

		public readonly int _cacheSize;

		public BTreeConfiguration(ITransactionalIdSystem idSystem, SlotChangeFactory slotChangeFactory
			, int cacheSize, bool canEnlistWithTransaction)
		{
			_idSystem = idSystem;
			_slotChangeFactory = slotChangeFactory;
			_canEnlistWithTransaction = canEnlistWithTransaction;
			_cacheSize = cacheSize;
		}

		public BTreeConfiguration(ITransactionalIdSystem idSystem, int cacheSize, bool canEnlistWithTransaction
			) : this(idSystem, SlotChangeFactory.SystemObjects, cacheSize, canEnlistWithTransaction
			)
		{
		}
	}
}
