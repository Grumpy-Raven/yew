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

I wanted to have the best of both worlds - the flexibility and familiarty of designing layouts using classes and CSS-like rules, and the speed of iteration using component driven architectures. So, Yew.

## But what does it look like??

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

Yew maintains a hierarchy of disposable view elements. I borrowed heavily from React's design [here](https://reactjs.org/blog/2015/12/18/react-components-elements-and-instances.html) (Note: What Yew calls "views", React calls "elements") It turns out, these things are pretty dang useful for reconciliation. Yew maintains a tree of Yew Nodes which each node is responsible for an associated VisualElement, a Yew View, and an instance of a Yew Component. Nodes also maintain a list of child nodes, and Yew uses an implementation of Longest Common Subsequence (yeah - all that dynamic programming interview study actually paid off - this is the first time I've ever had a reason to use a tabulation based dynamic programming algorithm in something that isn't just an interview problem) to reconcile children, adding, updating and inserting nodes as needed.

Unless the View derives from YewLib.Primitive (used for directly emitting VisualElements), it will probably contain a child Component type. This is where you actually implement UI logic, work with state, render a subtree, and so on. I played around with a number of different patterns here, and for me, it's all about ease of readability and speed of expressing my ideas through code. So, while I'm not normally a big fan of convention over configuration, being able to just know that there is a nested Component type makes the code really easy to write, and you don't have to come up with two names for everything - (naming things once is bad enough!)

## Render method

Components implement Render methods, just like React, which return a view hierarchy. In the counter sample, we return a Flex View (a ContainerView designed to easily align things horizontally instead of vertically), which contains a Label and a Button. Pretty easy right?

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
