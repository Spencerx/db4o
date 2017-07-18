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
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Db4objects.Db4o.Linq.Expressions
{
	internal abstract class ExpressionVisitor
	{
		protected virtual void Visit(Expression expression)
		{
			if (expression == null)
				return;

			switch (expression.NodeType)
			{
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.Not:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.ArrayLength:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
				case ExpressionType.UnaryPlus:
					VisitUnary((UnaryExpression)expression);
					break;
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.Divide:
				case ExpressionType.Power:
				case ExpressionType.Modulo:
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.Coalesce:
				case ExpressionType.ArrayIndex:
				case ExpressionType.RightShift:
				case ExpressionType.LeftShift:
				case ExpressionType.ExclusiveOr:
					VisitBinary((BinaryExpression)expression);
					break;
				case ExpressionType.TypeIs:
					VisitTypeIs((TypeBinaryExpression)expression);
					break;
				case ExpressionType.Conditional:
					VisitConditional((ConditionalExpression)expression);
					break;
				case ExpressionType.Constant:
					VisitConstant((ConstantExpression)expression);
					break;
				case ExpressionType.Parameter:
					VisitParameter((ParameterExpression)expression);
					break;
				case ExpressionType.MemberAccess:
					VisitMemberAccess((MemberExpression)expression);
					break;
				case ExpressionType.Call:
					VisitMethodCall((MethodCallExpression)expression);
					break;
				case ExpressionType.Lambda:
					VisitLambda((LambdaExpression)expression);
					break;
				case ExpressionType.New:
					VisitNew((NewExpression)expression);
					break;
				case ExpressionType.NewArrayInit:
				case ExpressionType.NewArrayBounds:
					VisitNewArray((NewArrayExpression)expression);
					break;
				case ExpressionType.Invoke:
					VisitInvocation((InvocationExpression)expression);
					break;
				case ExpressionType.MemberInit:
					VisitMemberInit((MemberInitExpression)expression);
					break;
				case ExpressionType.ListInit:
					VisitListInit((ListInitExpression)expression);
					break;
				default:
					throw new ArgumentException(string.Format("Unhandled expression type: '{0}'", expression.NodeType));
			}
		}

		protected virtual void VisitBinding(MemberBinding binding)
		{
			switch (binding.BindingType)
			{
				case MemberBindingType.Assignment:
					VisitMemberAssignment((MemberAssignment)binding);
					break;
				case MemberBindingType.MemberBinding:
					VisitMemberMemberBinding((MemberMemberBinding)binding);
					break;
				case MemberBindingType.ListBinding:
					VisitMemberListBinding((MemberListBinding)binding);
					break;
				default:
					throw new ArgumentException(string.Format("Unhandled binding type '{0}'", binding.BindingType));
			}
		}

		protected virtual void VisitElementInitializer(ElementInit initializer)
		{
			VisitExpressionList(initializer.Arguments);
		}

		protected virtual void VisitUnary(UnaryExpression unary)
		{
			Visit(unary.Operand);
		}

		protected virtual void VisitBinary(BinaryExpression binary)
		{
			Visit(binary.Left);
			Visit(binary.Right);
			Visit(binary.Conversion);
		}

		protected virtual void VisitTypeIs(TypeBinaryExpression type)
		{
			Visit(type.Expression);
		}

		protected virtual void VisitConstant(ConstantExpression constant)
		{
		}

		protected virtual void VisitConditional(ConditionalExpression conditional)
		{
			Visit(conditional.Test);
			Visit(conditional.IfTrue);
			Visit(conditional.IfFalse);
		}

		protected virtual void VisitParameter(ParameterExpression parameter)
		{
		}

		protected virtual void VisitMemberAccess(MemberExpression member)
		{
			Visit(member.Expression);
		}

		protected virtual void VisitMethodCall(MethodCallExpression methodCall)
		{
			Visit(methodCall.Object);
			VisitExpressionList(methodCall.Arguments);
		}

		protected virtual void VisitList<T>(ReadOnlyCollection<T> list, Action<T> visitor)
		{
			foreach (T element in list)
			{
				visitor(element);
			}
		}

		protected virtual void VisitExpressionList<TExp>(ReadOnlyCollection<TExp> list) where TExp : Expression
		{
			VisitList(list, Visit);
		}

		protected virtual void VisitMemberAssignment(MemberAssignment assignment)
		{
			Visit(assignment.Expression);
		}

		protected virtual void VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			VisitBindingList(binding.Bindings);
		}

		protected virtual void VisitMemberListBinding(MemberListBinding binding)
		{
			VisitElementInitializerList(binding.Initializers);
		}

		protected virtual void VisitBindingList<TBinding>(ReadOnlyCollection<TBinding> list) where TBinding : MemberBinding
		{
			VisitList(list, VisitBinding);
		}

		protected virtual void VisitElementInitializerList(ReadOnlyCollection<ElementInit> list)
		{
			VisitList(list, VisitElementInitializer);
		}

		protected virtual void VisitLambda(LambdaExpression lambda)
		{
			Visit(lambda.Body);
		}

		protected virtual void VisitNew(NewExpression nex)
		{
			VisitExpressionList(nex.Arguments);
		}

		protected virtual void VisitMemberInit(MemberInitExpression init)
		{
			VisitNew(init.NewExpression);
			VisitBindingList(init.Bindings);
		}

		protected virtual void VisitListInit(ListInitExpression init)
		{
			VisitNew(init.NewExpression);
			VisitElementInitializerList(init.Initializers);
		}

		protected virtual void VisitNewArray(NewArrayExpression newArray)
		{
			VisitExpressionList(newArray.Expressions);
		}

		protected virtual void VisitInvocation(InvocationExpression invocation)
		{
			VisitExpressionList(invocation.Arguments);
			Visit(invocation.Expression);
		}
	}
}
