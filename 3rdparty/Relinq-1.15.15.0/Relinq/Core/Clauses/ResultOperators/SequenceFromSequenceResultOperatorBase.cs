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
using ArangoDB.Client.Common.Remotion.Linq.Clauses.StreamedData;
using Remotion.Utilities;

namespace ArangoDB.Client.Common.Remotion.Linq.Clauses.ResultOperators
{
  /// <summary>
  /// Represents a <see cref="ResultOperatorBase"/> that is executed on a sequence, returning a new sequence as its result.
  /// </summary>
  public abstract class SequenceFromSequenceResultOperatorBase : ResultOperatorBase
  {
    public abstract StreamedSequence ExecuteInMemory<T> (StreamedSequence input);

    public override IStreamedData ExecuteInMemory (IStreamedData input)
    {
      ArgumentUtility.CheckNotNull ("input", input);
      return InvokeGenericExecuteMethod<StreamedSequence, StreamedSequence> (input, ExecuteInMemory<object>);
    }
  }
}
