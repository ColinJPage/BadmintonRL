using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/*!Interface that allows for easy event assigning to classes that derive from it
 * Subscribe and Unsubscribe methods take a generic action function
 */
public interface EventBroadcaster : ISubscribable<Action>
{

}
/*!Generic interface that allows for easy event assigning to classes that derive from it
 *  * Subscribe and Unsubscribe methods take a generic action function
 */
public interface EventBroadcaster<T> : ISubscribable<Action<T>>
{
}
public interface EventBroadcaster<T, U> : ISubscribable<Action<T, U>>
{
}

