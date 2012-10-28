namespace Cartographer.Compiler
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Dynamic;
	using System.Globalization;
	using System.IO;
	using System.Linq.Expressions;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using Cartographer.Internal.Expressions;
	using Cartographer.Internal.Extensions;
	using ExpressionType = System.Linq.Expressions.ExpressionType;

	sealed class DescriptionVisitor: ExpressionVisitor
	{
		readonly TextWriter writer;

		// Associate every unique label or anonymous parameter in the tree with an integer.
		// The label is displayed as Label_#.
		Dictionary<object, int> ids;

		public DescriptionVisitor(TextWriter writer)
		{
			this.writer = writer;
		}

		protected override Expression VisitBinary(BinaryExpression node)
		{
			var requiresParentheses = true;
			if (node.NodeType == ExpressionType.ArrayIndex)
			{
				Visit(node.Left);
				Append("[");
				Visit(node.Right);
				Append("]");
			}
			else
			{
				string op;
				switch (node.NodeType)
				{
					case ExpressionType.Assign:
						op = "=";
						requiresParentheses = false;
						break;
					case ExpressionType.Equal:
						op = "==";
						break;
					case ExpressionType.NotEqual:
						op = "!=";
						break;
					case ExpressionType.AndAlso:
						op = "AndAlso";
						break;
					case ExpressionType.OrElse:
						op = "OrElse";
						break;
					case ExpressionType.GreaterThan:
						op = ">";
						break;
					case ExpressionType.LessThan:
						op = "<";
						break;
					case ExpressionType.GreaterThanOrEqual:
						op = ">=";
						break;
					case ExpressionType.LessThanOrEqual:
						op = "<=";
						break;
					case ExpressionType.Add:
						op = "+";
						break;
					case ExpressionType.AddAssign:
						op = "+=";
						break;
					case ExpressionType.AddAssignChecked:
						op = "+=";
						break;
					case ExpressionType.AddChecked:
						op = "+";
						break;
					case ExpressionType.Subtract:
						op = "-";
						break;
					case ExpressionType.SubtractAssign:
						op = "-=";
						break;
					case ExpressionType.SubtractAssignChecked:
						op = "-=";
						break;
					case ExpressionType.SubtractChecked:
						op = "-";
						break;
					case ExpressionType.Divide:
						op = "/";
						break;
					case ExpressionType.DivideAssign:
						op = "/=";
						break;
					case ExpressionType.Modulo:
						op = "%";
						break;
					case ExpressionType.ModuloAssign:
						op = "%=";
						break;
					case ExpressionType.Multiply:
						op = "*";
						break;
					case ExpressionType.MultiplyAssign:
						op = "*=";
						break;
					case ExpressionType.MultiplyAssignChecked:
						op = "*=";
						break;
					case ExpressionType.MultiplyChecked:
						op = "*";
						break;
					case ExpressionType.LeftShift:
						op = "<<";
						break;
					case ExpressionType.LeftShiftAssign:
						op = "<<=";
						break;
					case ExpressionType.RightShift:
						op = ">>";
						break;
					case ExpressionType.RightShiftAssign:
						op = ">>=";
						break;
					case ExpressionType.And:
						if (node.Type == typeof (bool) || node.Type == typeof (bool?))
						{
							op = "And";
						}
						else
						{
							op = "&";
						}
						break;
					case ExpressionType.AndAssign:
						if (node.Type == typeof (bool) || node.Type == typeof (bool?))
						{
							op = "&&=";
						}
						else
						{
							op = "&=";
						}
						break;
					case ExpressionType.Or:
						if (node.Type == typeof (bool) || node.Type == typeof (bool?))
						{
							op = "Or";
						}
						else
						{
							op = "|";
						}
						break;
					case ExpressionType.OrAssign:
						if (node.Type == typeof (bool) || node.Type == typeof (bool?))
						{
							op = "||=";
						}
						else
						{
							op = "|=";
						}
						break;
					case ExpressionType.ExclusiveOr:
						op = "^";
						break;
					case ExpressionType.ExclusiveOrAssign:
						op = "^=";
						break;
					case ExpressionType.Power:
						op = "^";
						break;
					case ExpressionType.PowerAssign:
						op = "**=";
						break;
					case ExpressionType.Coalesce:
						op = "??";
						break;

					default:
						throw new InvalidOperationException();
				}
				if (requiresParentheses)
				{
					Append("(");
				}
				Visit(node.Left);
				Append(' ');
				Append(op);
				Append(' ');
				Visit(node.Right);
				if (requiresParentheses)
				{
					Append(")");
				}
			}
			return node;
		}

		protected override Expression VisitBlock(BlockExpression node)
		{
			AppendLine("{");
			foreach (var expression in node.Expressions)
			{
				Visit(expression);
				AppendLine();
			}
			AppendLine("}");
			return node;
		}

		protected override CatchBlock VisitCatchBlock(CatchBlock node)
		{
			Append("catch (" + node.Test.Name);
			if (node.Variable != null)
			{
				Append(node.Variable.Name ?? "");
			}
			Append(") { ... }");
			return node;
		}

		protected override Expression VisitConditional(ConditionalExpression node)
		{
			Append("if(");
			Visit(node.Test);
			Append(", ");
			Visit(node.IfTrue);
			var defaultElse = node.IfFalse as DefaultExpression;
			if (defaultElse == null || defaultElse.Type != typeof (void))
			{
				Append(", ");
				Visit(node.IfFalse);
			}
			Append(")");
			return node;
		}

		protected override Expression VisitConstant(ConstantExpression node)
		{
			if (node.Value != null)
			{
				var sValue = node.Value.ToString();
				if (node.Value is string)
				{
					Append("\"");
					Append(sValue);
					Append("\"");
				}
				else if (sValue == node.Value.GetType().ToString())
				{
					Append("value(");
					Append(sValue);
					Append(")");
				}
				else
				{
					Append(sValue);
				}
			}
			else
			{
				Append("null");
			}
			return node;
		}

		protected override Expression VisitDebugInfo(DebugInfoExpression node)
		{
			var s = String.Format(
				CultureInfo.CurrentCulture,
				"<DebugInfo({0}: {1}, {2}, {3}, {4})>",
				node.Document.FileName,
				node.StartLine,
				node.StartColumn,
				node.EndLine,
				node.EndColumn
				);
			Append(s);
			return node;
		}

		protected override Expression VisitDefault(DefaultExpression node)
		{
			Append("default(");
			Append(node.Type.Name);
			Append(")");
			return node;
		}

		protected override Expression VisitDynamic(DynamicExpression node)
		{
			Append(FormatBinder(node.Binder));
			VisitExpressions('(', node.Arguments, ')');
			return node;
		}

		protected override ElementInit VisitElementInit(ElementInit initializer)
		{
			Append(initializer.AddMethod.ToString());
			VisitExpressions('(', initializer.Arguments, ')');
			return initializer;
		}

		protected override Expression VisitExtension(Expression node)
		{
			dynamic custom = node;
			return VisitCustom(custom);
		}

		protected override Expression VisitGoto(GotoExpression node)
		{
			Append(node.Kind.ToString().ToLower());
			DumpLabel(node.Target);
			if (node.Value != null)
			{
				Append(" (");
				Visit(node.Value);
				Append(") ");
			}
			return node;
		}

		protected override Expression VisitIndex(IndexExpression node)
		{
			if (node.Object != null)
			{
				Visit(node.Object);
			}
			else
			{
				Debug.Assert(node.Indexer != null);
				Append(node.Indexer.DeclaringType.Name);
			}
			if (node.Indexer != null)
			{
				Append(".");
				Append(node.Indexer.Name);
			}

			VisitExpressions('[', node.Arguments, ']');
			return node;
		}

		protected override Expression VisitInvocation(InvocationExpression node)
		{
			Append("Invoke(");
			Visit(node.Expression);
			for (int i = 0, n = node.Arguments.Count; i < n; i++)
			{
				Append(", ");
				Visit(node.Arguments[i]);
			}
			Append(")");
			return node;
		}

		protected override Expression VisitLabel(LabelExpression node)
		{
			Append("{ ... } ");
			DumpLabel(node.Target);
			Append(":");
			return node;
		}

		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			if (node.Parameters.Count == 1)
			{
				// p => body 
				Visit(node.Parameters[0]);
			}
			else
			{
				// (p1, p2, ..., pn) => body 
				VisitExpressions('(', node.Parameters, ')');
			}
			Append(" => ");
			Visit(node.Body);
			return node;
		}

		protected override Expression VisitListInit(ListInitExpression node)
		{
			Visit(node.NewExpression);
			Append(" {");
			for (int i = 0, n = node.Initializers.Count; i < n; i++)
			{
				if (i > 0)
				{
					Append(", ");
				}
				Append(node.Initializers[i].ToString());
			}
			Append("}");
			return node;
		}

		protected override Expression VisitLoop(LoopExpression node)
		{
			Append("loop { ... }");
			return node;
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			OutMember(node.Expression, node.Member);
			return node;
		}

		protected override MemberAssignment VisitMemberAssignment(MemberAssignment assignment)
		{
			Append(assignment.Member.Name);
			Append(" = ");
			Visit(assignment.Expression);
			return assignment;
		}

		protected override Expression VisitMemberInit(MemberInitExpression node)
		{
			if (node.NewExpression.Arguments.Count == 0 &&
			    node.NewExpression.Type.Name.Contains("<"))
			{
				// anonymous type constructor 
				Append("new");
			}
			else
			{
				Visit(node.NewExpression);
			}
			Append(" {");
			for (int i = 0, n = node.Bindings.Count; i < n; i++)
			{
				var b = node.Bindings[i];
				if (i > 0)
				{
					Append(", ");
				}
				VisitMemberBinding(b);
			}
			Append("}");
			return node;
		}

		protected override MemberListBinding VisitMemberListBinding(MemberListBinding binding)
		{
			Append(binding.Member.Name);
			Append(" = {");
			for (int i = 0, n = binding.Initializers.Count; i < n; i++)
			{
				if (i > 0)
				{
					Append(", ");
				}
				VisitElementInit(binding.Initializers[i]);
			}
			Append("}");
			return binding;
		}

		protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding binding)
		{
			Append(binding.Member.Name);
			Append(" = {");
			for (int i = 0, n = binding.Bindings.Count; i < n; i++)
			{
				if (i > 0)
				{
					Append(", ");
				}
				VisitMemberBinding(binding.Bindings[i]);
			}
			Append("}");
			return binding;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			var start = 0;
			var ob = node.Object;

			if (Attribute.GetCustomAttribute(node.Method, typeof (ExtensionAttribute)) != null)
			{
				start = 1;
				ob = node.Arguments[0];
			}

			if (ob != null)
			{
				Visit(ob);
				Append(".");
			}
			Append(node.Method.Name);
			Append("(");
			for (int i = start, n = node.Arguments.Count; i < n; i++)
			{
				if (i > start)
				{
					Append(", ");
				}
				Visit(node.Arguments[i]);
			}
			Append(")");
			return node;
		}

		protected override Expression VisitNew(NewExpression node)
		{
			Append("new " + node.Type.Name);
			Append("(");
			for (var i = 0; i < node.Arguments.Count; i++)
			{
				if (i > 0)
				{
					Append(", ");
				}
				if (node.Members != null)
				{
					Append(node.Members[i].Name);
					Append(" = ");
				}
				Visit(node.Arguments[i]);
			}
			Append(")");
			return node;
		}

		protected override Expression VisitNewArray(NewArrayExpression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.NewArrayBounds:
					// new MyType[](expr1, expr2)
					Append("new " + node.Type);
					VisitExpressions('(', node.Expressions, ')');
					break;
				case ExpressionType.NewArrayInit:
					// new [] {expr1, expr2} 
					Append("new [] ");
					VisitExpressions('{', node.Expressions, '}');
					break;
			}
			return node;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			if (node.IsByRef)
			{
				Append("ref ");
			}
			if (String.IsNullOrEmpty(node.Name))
			{
				var id = GetParamId(node);
				Append("Param_" + id);
			}
			else
			{
				Append(node.Name);
			}
			return node;
		}

		protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
		{
			VisitExpressions('(', node.Variables, ')');
			return node;
		}

		protected override Expression VisitSwitch(SwitchExpression node)
		{
			Append("switch ");
			Append("(");
			Visit(node.SwitchValue);
			Append(") { ... }");
			return node;
		}

		protected override SwitchCase VisitSwitchCase(SwitchCase node)
		{
			Append("case ");
			VisitExpressions('(', node.TestValues, ')');
			Append(": ...");
			return node;
		}

		protected override Expression VisitTry(TryExpression node)
		{
			Append("try { ... }");
			return node;
		}

		protected override Expression VisitTypeBinary(TypeBinaryExpression node)
		{
			Append("(");
			Visit(node.Expression);
			switch (node.NodeType)
			{
				case ExpressionType.TypeIs:
					Append(" Is ");
					break;
				case ExpressionType.TypeEqual:
					Append(" TypeEqual ");
					break;
			}
			Append(node.TypeOperand.Name);
			Append(")");
			return node;
		}

		protected override Expression VisitUnary(UnaryExpression node)
		{
			switch (node.NodeType)
			{
				case ExpressionType.TypeAs:
					Append("(");
					break;
				case ExpressionType.Not:
					Append("Not(");
					break;
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
					Append("-");
					break;
				case ExpressionType.UnaryPlus:
					Append("+");
					break;
				case ExpressionType.Quote:
					break;
				case ExpressionType.Throw:
					Append("throw(");
					break;
				case ExpressionType.Increment:
					Append("Increment(");
					break;
				case ExpressionType.Decrement:
					Append("Decrement(");
					break;
				case ExpressionType.PreIncrementAssign:
					Append("++");
					break;
				case ExpressionType.PreDecrementAssign:
					Append("--");
					break;
				case ExpressionType.OnesComplement:
					Append("~(");
					break;
				default:
					Append(node.NodeType.ToString());
					Append("(");
					break;
			}

			Visit(node.Operand);

			switch (node.NodeType)
			{
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.UnaryPlus:
				case ExpressionType.PreDecrementAssign:
				case ExpressionType.PreIncrementAssign:
				case ExpressionType.Quote:
					break;
				case ExpressionType.TypeAs:
					Append(" As ");
					Append(node.Type.Name);
					Append(")");
					break;
				case ExpressionType.PostIncrementAssign:
					Append("++");
					break;
				case ExpressionType.PostDecrementAssign:
					Append("--");
					break;
				default:
					Append(")");
					break;
			}
			return node;
		}

		void AddLabel(LabelTarget label)
		{
			if (ids == null)
			{
				ids = new Dictionary<object, int>();
				ids.Add(label, 0);
			}
			else
			{
				if (!ids.ContainsKey(label))
				{
					ids.Add(label, ids.Count);
				}
			}
		}

		void AddParam(ParameterExpression p)
		{
			if (ids == null)
			{
				ids = new Dictionary<object, int>();
				ids.Add(ids, 0);
			}
			else
			{
				if (!ids.ContainsKey(p))
				{
					ids.Add(p, ids.Count);
				}
			}
		}

		void Append(string s)
		{
			writer.Write(s);
		}

		void Append(char c)
		{
			writer.Write(c);
		}

		void AppendLine()
		{
			writer.WriteLine();
		}

		void AppendLine(string s)
		{
			writer.WriteLine(s);
		}

		void DumpLabel(LabelTarget target)
		{
			if (!String.IsNullOrEmpty(target.Name))
			{
				Append(target.Name);
			}
			else
			{
				var labelId = GetLabelId(target);
				Append("UnamedLabel_" + labelId);
			}
		}

		int GetLabelId(LabelTarget label)
		{
			if (ids == null)
			{
				ids = new Dictionary<object, int>();
				AddLabel(label);
				return 0;
			}
			else
			{
				int id;
				if (!ids.TryGetValue(label, out id))
				{
					//label is met the first time 
					id = ids.Count;
					AddLabel(label);
				}
				return id;
			}
		}

		int GetParamId(ParameterExpression p)
		{
			if (ids == null)
			{
				ids = new Dictionary<object, int>();
				AddParam(p);
				return 0;
			}
			else
			{
				int id;
				if (!ids.TryGetValue(p, out id))
				{
					// p is met the first time 
					id = ids.Count;
					AddParam(p);
				}
				return id;
			}
		}


		void OutMember(Expression instance, MemberInfo member)
		{
			if (instance != null)
			{
				Visit(instance);
				Append("." + member.Name);
			}
			else
			{
				// For static members, include the type name
				Append(member.DeclaringType.Name + "." + member.Name);
			}
		}

		void OutRightHandSideOf(PropertyIfNotNullExpression owner)
		{
			Visit(owner.Owner);
			Append("?!");
		}

		Expression VisitCustom(PropertyIfNotNullInnerExpression node)
		{
			OutRightHandSideOf(node.Owner);
			Append(node.Inner.Member.Name);
			return node;
		}

		Expression VisitCustom(PropertyIfNotNullExpression node)
		{
			var member = node.Inner as MemberExpression;
			if (member != null)
			{
				Append(member.Member.Name);
			}
			var convert = node.Inner as UnaryExpression;
			if (convert != null && convert.NodeType == ExpressionType.Convert)
			{
				Visit(convert.Operand);
			}
			else
			{
				Visit(node.Inner);
			}
			return node;
		}

		Expression VisitCustom(Expression node)
		{
			// Prefer an overriden ToString, if available. 
			var toString = node.GetType().GetMethod("ToString", TypeExtensions.EmptyTypes);
			if (toString.DeclaringType != typeof (Expression))
			{
				Append(node.ToString());
				return node;
			}

			Append("[");
			// For 3.5 subclasses, print the NodeType. 
			// For Extension nodes, print the class name.
			if (node.NodeType == ExpressionType.Extension)
			{
				Append(node.GetType().FullName);
			}
			else
			{
				Append(node.NodeType.ToString());
			}
			Append("]");
			return node;
		}

		void VisitExpressions<T>(char open, IList<T> expressions, char close) where T: Expression
		{
			Append(open);
			if (expressions != null)
			{
				var isFirst = true;
				foreach (var e in expressions)
				{
					if (isFirst)
					{
						isFirst = false;
					}
					else
					{
						Append(", ");
					}
					Visit(e);
				}
			}
			Append(close);
		}

		static string FormatBinder(CallSiteBinder binder)
		{
			ConvertBinder convert;
			GetMemberBinder getMember;
			SetMemberBinder setMember;
			DeleteMemberBinder deleteMember;
			GetIndexBinder getIndex;
			SetIndexBinder setIndex;
			DeleteIndexBinder deleteIndex;
			InvokeMemberBinder call;
			InvokeBinder invoke;
			CreateInstanceBinder create;
			UnaryOperationBinder unary;
			BinaryOperationBinder binary;

			if ((convert = binder as ConvertBinder) != null)
			{
				return "Convert " + convert.Type;
			}
			else if ((getMember = binder as GetMemberBinder) != null)
			{
				return "GetMember " + getMember.Name;
			}
			else if ((setMember = binder as SetMemberBinder) != null)
			{
				return "SetMember " + setMember.Name;
			}
			else if ((deleteMember = binder as DeleteMemberBinder) != null)
			{
				return "DeleteMember " + deleteMember.Name;
			}
			else if ((getIndex = binder as GetIndexBinder) != null)
			{
				return "GetIndex";
			}
			else if ((setIndex = binder as SetIndexBinder) != null)
			{
				return "SetIndex";
			}
			else if ((deleteIndex = binder as DeleteIndexBinder) != null)
			{
				return "DeleteIndex";
			}
			else if ((call = binder as InvokeMemberBinder) != null)
			{
				return "Call " + call.Name;
			}
			else if ((invoke = binder as InvokeBinder) != null)
			{
				return "Invoke";
			}
			else if ((create = binder as CreateInstanceBinder) != null)
			{
				return "Create";
			}
			else if ((unary = binder as UnaryOperationBinder) != null)
			{
				return unary.Operation.ToString();
			}
			else if ((binary = binder as BinaryOperationBinder) != null)
			{
				return binary.Operation.ToString();
			}
			else
			{
				return "CallSiteBinder";
			}
		}
	}
}