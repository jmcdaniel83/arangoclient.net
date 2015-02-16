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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ArangoDB.Client.Common.Remotion.Linq.Clauses;
using Remotion.Utilities;

namespace ArangoDB.Client.Common.Remotion.Linq.Parsing.Structure.IntermediateModel
{
  /// <summary>
  /// Represents a <see cref="MethodCallExpression"/> for 
  /// <see cref="Queryable.Where{TSource}(System.Linq.IQueryable{TSource},System.Linq.Expressions.Expression{System.Func{TSource,bool}})"/>.
  /// It is generated by <see cref="ExpressionTreeParser"/> when an <see cref="Expression"/> tree is parsed.
  /// </summary>
  public class WhereExpressionNode : MethodCallExpressionNodeBase
  {
    public static readonly MethodInfo[] SupportedMethods = new[]
                                                           {
                                                               GetSupportedMethod (() => Queryable.Where<object> (null, o => true)),
                                                               GetSupportedMethod (() => Enumerable.Where<object> (null, o => true)),
                                                           };

    private readonly ResolvedExpressionCache<Expression> _cachedPredicate;

    public WhereExpressionNode (MethodCallExpressionParseInfo parseInfo, LambdaExpression predicate)
        : base (parseInfo)
    {
      ArgumentUtility.CheckNotNull ("predicate", predicate);

      if (predicate.Parameters.Count != 1)
        throw new ArgumentException ("Predicate must have exactly one parameter.", "predicate");

      Predicate = predicate;
      _cachedPredicate = new ResolvedExpressionCache<Expression> (this);
    }

    public LambdaExpression Predicate { get; private set; }

    public Expression GetResolvedPredicate (ClauseGenerationContext clauseGenerationContext)
    {
      return _cachedPredicate.GetOrCreate (r => r.GetResolvedExpression (Predicate.Body, Predicate.Parameters[0], clauseGenerationContext));
    }

    public override Expression Resolve (
        ParameterExpression inputParameter, Expression expressionToBeResolved, ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull ("inputParameter", inputParameter);
      ArgumentUtility.CheckNotNull ("expressionToBeResolved", expressionToBeResolved);

      // this simply streams its input data to the output without modifying its structure, so we resolve by passing on the data to the previous node
      return Source.Resolve (inputParameter, expressionToBeResolved, clauseGenerationContext);
    }

    protected override QueryModel ApplyNodeSpecificSemantics (QueryModel queryModel, ClauseGenerationContext clauseGenerationContext)
    {
      ArgumentUtility.CheckNotNull ("queryModel", queryModel);

      var clause = new WhereClause (GetResolvedPredicate (clauseGenerationContext));
      queryModel.BodyClauses.Add (clause);
      return queryModel;
    }
  }
}
