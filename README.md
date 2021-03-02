<p align="center">
  <img src="https://user-images.githubusercontent.com/309808/109713137-af64fd80-7b55-11eb-9844-38d8d51fe9b0.png" />
</p>

<h1 align="center">yew</h1>

Yew is a library to help make it easier to write interactive UI for Unity. Get it? Yew...nity?

It's inspired by things like [React](https://reactjs.org/), [Elm](https://guide.elm-lang.org/architecture/), [Recoil](https://recoiljs.org/), and [.NET MAUI](https://devblogs.microsoft.com/dotnet/introducing-net-multi-platform-app-ui/#mvu).

## Rationale
There's a lot to like with the upcoming support for UI Toolkit in the Unity runtime. Coming from a web developer background, the similarity between HTML/CSS & UXML/USS is just wonderful. But, I've been using libraries like React for so long that I actually don't really love handediting UXML and digging around with selector queries to find nodes in my C# code, and the steps it seems to take to get custom components created and usable in the UI Builder tool just feel too heavy. 

Coming from a world where creating a functional component is as easy as:

```javascript
var OneTwoThree({ num }) => <div>{num}</div>
```

I wanted to have the flexibility and familiarity of designing layouts using classes and CSS-like rules, and the speed of iteration using component driven architectures. So, Yew.

## What does it look like?

Let's start with a simple example, ye old counter:

```csharp
public class CounterApp : View
{
    public class Component : YewLib.Component
    {
        public override View Render()
        {
            var state = UseState(0);
            return new Flex()
            {
                Label(state.Value.ToString()),
                Button("Increment", () => state.Value++)
            };
        }
    }
}
```

And to plant it into a UI Document, from a MonoBehavior:

```csharp
var uiDoc = GetComponent<UIDocument>();
Yew.Render(new Examples(), uiDoc.rootVisualElement);
```

Shall we talk a bit about what's going on here?

### We have a CounterApp View, and a CounterApp.Component.

Yew maintains a hierarchy of disposable view elements. I borrowed heavily from React's design [here](https://reactjs.org/blog/2015/12/18/react-components-elements-and-instances.html) (Note: What Yew calls "views", React calls "elements") It turns out, these things are pretty dang useful for reconciliation. Yew maintains a tree of Yew Nodes which each node is responsible for an associated VisualElement, a Yew View, and an instance of a Yew Component. Nodes also maintain a list of child nodes, and Yew uses an implementation of Longest Common Subsequence (yeah - all that dynamic programming interview study actually paid off - this is the first time I've ever had a reason to use a tabulation based dynamic programming algorithm in something that isn't just an interview problem) to reconcile children, adding, updating and inserting nodes as needed. (Note: this is different than what React does, or did at least, per this [doc on react reconiliation](https://reactjs.org/docs/reconciliation.html). I couldn't find a great discussion of how modern react reconciles, if anyone has insight here, I'd love to hear it. Yew's LCS reconciler is O(m*n^2) where m is the number of nodes and n is the average number of children per node, whereas react claims to have a linear time reconciler, which I'm definitely curious about.)

Unless the View derives from YewLib.Primitive (used for directly emitting VisualElements), it will probably contain a child Component type. This is where you actually implement UI logic, work with state, render a subtree, and so on. I played around with a number of different patterns here, and for me, it's all about ease of readability and speed of expressing my ideas through code. So, while I'm not normally a big fan of convention over configuration, being able to just know that there is a nested Component type makes the code really easy to write, and you don't have to come up with two names for everything - (naming things once is bad enough!)

### Render method

Components implement Render methods, just like React, which return a view hierarchy. In the counter sample, we return a Flex View (a ContainerView designed to easily align things horizontally instead of vertically), which contains a Label and a Button. Pretty easy right? When the button is clicked, the state is updated, which triggers a component update, and voila! you have an updated label.

### Wait - is that a useState hook?

It is! I am a fan of react's [hooks](https://reactjs.org/docs/hooks-intro.html) and found that putting them into Yew wasn't too difficult. C#'s syntax sugar makes them easy to use too.

You can also call UseState inside of "method components":

```csharp
View ItemRenderer(int i)
{
    var state = UseState(0);
    return new StackLayout()
    {
        Label($"Counter #{i}: {state.Value}"),
        Button("click me random", () => state.Value++),
    };
}

public override View Render() => new StackLayout {
  ItemRenderer(1),
  ItemRenderer(2),
}
```

"Method components" aren't quite like React's functional components, for instance, they currently aren't capable of being indepdendently updated by Yew (they update when their parent Component updates). But they can independently store state. I'm planning on using them for laying out lighter weight items in my UI, and when they get bigger, I'll probably move them into their own full fledged View/Components.

### What about global state?

I tried playing around with some of the various attempts at porting Redux over to C# and Unity (if anyone wants to give this a go, I'd love to post a sample on how to do that). I do think that when Unity supports C# 9.0, some nice redux-y stuff will be easier to do with the new record types.

I did stumble across [recoiljs](https://recoiljs.org), and the "atom" pattern was pretty easy to layer in.

```csharp
var state = UseAtom(TodoAppKey, () => new TodoState());
```

What this does is creates a global bit of state, which lives independently of the component. So, if this component gets cleaned up, you don't lose the state. Nice! If you want to dig in a little more into Yew atoms, have a look at the [Todo App](https://github.com/Grumpy-Raven/yew/blob/main/samples/TodoApp.cs#L40) (and you can see how to 'subscribe' to the atom value in the [hello world sample](https://github.com/Grumpy-Raven/yew/blob/main/samples/HelloWorld.cs#L28)). 

Note also, you can use lambdas for more complex state constructors.
Another note: atoms don't currently garbage collect. Let's call that a TODO shall we?

### Anything Else?

A few little tidbits I think are worth pointing out.

#### Use with switch expressions
```csharp
  View SampleChoice(Choice choice) => choice switch
  {
      Choice.Counter => new CounterApp(),
      Choice.Todo => new TodoApp(),
      Choice.HelloWorld => new HelloWorld(),
      _ => new Label("Choose a sample to learn more about yew")
  }
```

#### Render data item collections with Select and Method Components
```csharp
return new StackLayout()
{
   state.Value.TodoItems
     .Where(x => !x.Completed)
     .Select(item => TodoItemView(state.Update, item))
```

#### HTML / React style attributes
We do these in constructors, rather than object initializers, so as not to conflict with list initialization. Fortunately, named optional parameters make this very pleasant to do:
```
return new StackLayout(className: "root", style: "UI/styles.uss")            
```

Oh yeah, that's how we get styles associated. Which is useful. I don't fully know my way around UI Toolkit yet, but I think Yew should inherit pretty much all of the functioanlity of UI Toolkit, well, except for UI Builder. But, you can edit your stylesheets and see those changes get reflected in real time, and you can use the nifty UI Toolkit Debugger tool. Editing the C# does not give a good hot reload experience, at least I haven't figured that one out very well yet.

## Installation

Yew is available as a Unity package. [link]

## Examples

Are you looking for an example project to get started?
 * [Counter](https://github.com/Grumpy-Raven/yew/blob/main/samples/CounterApp.cs)
 * [Todo](https://github.com/Grumpy-Raven/yew/blob/main/samples/TodoApp.cs)
 * [Samples Browser](https://github.com/Grumpy-Raven/yew/blob/main/samples/Examples.cs)

## License

This project is licensed under the terms of the
[MIT license](/LICENSE).
