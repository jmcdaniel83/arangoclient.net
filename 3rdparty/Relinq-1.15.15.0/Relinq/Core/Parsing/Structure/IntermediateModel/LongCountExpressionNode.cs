// Copyright (c) rubicon IT GmbH, www.rubicon.eu
//
// See the NOTICE file distributed with this work for additional information
// regarding copyright ownership.  rubicon licenses this file to you under 
// the Apache License, Version 2.0 (the "License"); you may not use this 
// file except in compliance with the License.  You may obtain a copy of the 
// License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  See the 
// License for the specific language governing permissions and limitations
// under the License.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ArangoDB.Client.Common.Remotion.Linq.Clauses;
using ArangoDB.Client.Common.Remotion.Linq.Clauses.ResultOperators;

namespace ArangoDB.Client.Common.Remotion.Linq.Parsing.Structure.IntermediateModel
{
  /// <summary>
  /// Represents a <see cref="MethodCallExpression"/> for <see cref="Queryable.LongCount{TSource}(System.Linq.IQueryable{TSource})"/>,
  /// <see cref="Queryable.LongCount{TSource}(System.Linq.IQueryable{TSource},System.Linq.Expressions.Expression{System.Func{TSource,bool}})"/>,
  /// and for the <see cref="Array.Length"/> property of arrays.
  /// It is generated by <see cref="ExpressionTreeParser"/> when an <see cref="Expression"/> tree is parsed.
  /// When this node is used, it marks the beginning (i.e. the last node) of an <see cref="IExpressionNode"/> chain that represents a query.
  /// </summary>
  public class LongCountExpressionNode : ResultOperatorExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods;

    static LongCountExpressionNode ()
    {
      var supportedMethods = new List<MethodInfo>
                             {
                             GetSupportedMethod (() => Queryable.LongCount<object> (null)),
                             GetSupportedMethod (() => Queryable.LongCount<object> (null, null)),
                             GetSupportedMethod (() => Enumerable.LongCount<object> (null)),
                             GetSupportedMethod (() => Enumerable.LongCount<object> (null, null)),
                         };

      var arrayLongLengthExpression = GetArrayLongLengthExpression();
      if (arrayLongLengthExpression != null)
        supportedMethods.Add (GetSupportedMethod (arrayLongLengthExpression));

      SupportedMethods = supportedMethods.ToArray();
    }

    private static Expression<Func<long>> GetArrayLongLengthExpression ()
    {
      var property = typeof (Array).GetRuntimeProperty ("LongLength");
      if (property == null)
        return null;

      //() => (((Array) null).LongLength;
      return Expression.Lambda<Func<long>>(Expression.MakeMemberAccess (Expression.Constant (null, typeof (Array)), property));
    }

    public LongCountExpressionNode (MethodCallExpressionParseInfo parseInfo, LambdaExpression optionalPredicate)
        : base (parseInfo, optionalPredicate, null)
    {
    }

    public override Expression Resolve (
        ParameterExpression inputParameter, Expression expressionToBeResolved, ClauseGenerationContext clauseGenerationContext)
    {
      // no data streams out from this node, so we cannot resolve any expressions
      throw CreateResolveNotSupportedException();
    }

    protected override ResultOperatorBase CreateResultOperator (ClauseGenerationContext clauseGenerationContext)
    {
      return new LongCountResultOperator();
    }
  }
}
