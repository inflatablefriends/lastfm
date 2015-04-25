# Dependency Injection

The SDK is built to work with IoC libraries like MvvmLight and Ninject. To inject an API as a dependency to a viewmodel, you just need to register ```ILastAuth``` to an instance of ```LastAuth```:

```c#
// mvvmlight
var auth = new LastAuth("apikey", "apisecret");
SimpleIoc.Default.Register<ILastAuth>(() => auth);

// ...

var artistApi = ServiceLocator.Current.GetInstance<ArtistApi>();
var response = await artistApi.GetArtistInfoAsync("The Knife");
var theKnife = artist.Content;
```