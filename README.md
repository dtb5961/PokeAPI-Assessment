# PokeAPI Assessement

## Project Overview

This Console app will take a Pokemon Name and generate a list of the pokemon types it is strong against and weak against.

The user will be prompted to enter a pokemon, if the pokemon exists then type info will be retrieved and results will be displayed. If the Pokemon isn't found then a suggestion list will be displayed and the user can retrieve those options. If there is an error from the PokeApi then the message will be displated and the program will end. Alternatively if the user enters nothing then the program will also end

Poke API will be used https://pokeapi.co/docs/v2#info and The FuzzySearch C# package is used to generate a list of suggested Pokemon.

## Architecture Sumamry

Program.CS - contains the setup for the application including the dependency injection for the httpClient and the PokemonService used to communicate with PokeApi

AppRunner - contains the orchestration and user I/O. It get user input and cleans and displays the results.

### Services

Pokemon Services contains the following

1. GetPokemonAttributes
   - GET https://pokeapi.co/api/v2/pokemon/{id or name}/
   - Used to retrieve the pokemon information for the user selected pokemon as well as get the type infomation used in GetTypeEffectInfo
2. GetPokedex
   - GET https://pokeapi.co/api/v2/pokemon?limit=-1
   - Used to retrieve a list of all available pokemon in the PokeAPI. This info is used by Fuzzy Search to suggest Pokemon that they user may have meant.
3. GetTypeEffectInfo
   - GET https://pokeapi.co/api/v2/type/{id or name}/
   - Used to retrieve the status effect info using the user selected Pokemon type. As a note there may be multiple types depending on the Pokemon

### Models

The models are used to map the responses from the PokeAPI into usable C# objects. There is also a corresponding PokeAPI result model for each service call to handle HTTP success or failures. As a rule GetPokemonAttribute NotFound is recoverable by FuzzySearch Suggestions but the other two results generally point to some issue with PokeAPI so the Status code is logged and the program exits.

### Utility

There is a Utility class added for general data processing of the object received from the PokeAPI http calls. The Utility class is used as a clear separation of responsibilites and to generally make debugging easier. If there is an HTTP failure then it is captured in the service and output in the AppRunner.

The Utility class on the otherhand handles all of the heavy handed data manipulation and any errors in it will reflect in the stacktrace.

## Run Instructions

There are a few packages that should be handled during the build process , those packages can be found in the Csproj file in the main applications.

The following will build and run the application

```
dotnet build
dotnet run
```

Sample Output

```
Enter a Pokémon name (or press Enter to exit):
Pikachu

Pokemon is pikachu and type/s is/are: electric

Strong against: flying, water, steel, electric
Weak against: ground, grass, electric, dragon

Enter a Pokémon name (or press Enter to exit):
Char

No Pokémon found for 'Char'.
Did you mean:
 1. chimchar
 2. charizard
 3. charmander
 4. charjabug
 5. charmeleon

Enter the number of the Pokémon you meant, or press Enter to cancel:
2

Fetching data for 'charizard'...

Pokemon is charizard and type/s is/are: fire, flying

Strong against: bug, steel, grass, ice, fire, fairy, fighting, ground
Weak against: rock, fire, water, dragon, ground, steel, electric, ice

Enter a Pokémon name (or press Enter to exit):
Wooper

Pokemon is wooper and type/s is/are: water, ground

Strong against: ground, rock, fire, steel, water, ice, poison, electric
Weak against: water, grass, dragon, electric, flying, bug, ice

Enter a Pokémon name (or press Enter to exit):

No Pokémon Entered Exiting
```

## Testing

Unit tests were created for the Service class and the Utility class.
A resources folder was added with responses from the PokeAPI app as of December 2025 for verification.

The Apprunner and Program files were skipped since its just setup and app flow.

MockRunner and Moq were used to mock the HTTPClient and the responses received from the API calls.

The following command should be run in the project test folder. A coverage report is also generated but it may also include the uncovered percentages from the Apprunner.cs and Program.cs

```
dotnet test
```

## Future Improvements

- The Pokedex could be cached to reduce the Pokedex call as a saved resource file. It currently isnt because of the simplicity of the project and lack of versioning from the PokeAPI. Without versioning, there is a risk that the PokeAPI is updated but the suggestions list would rely on older data.

- FuzzySearch could also optimized, for now it is the cause of a 1-2 second delay when the user misspells a Pokemon name. FuzzySearch was the suggested package but there seems to be others available or other methods/tighter constraints could be used to reduce processing time.

- Error handling could also be expanded but it is local application with an external endpoint as the backbone of the application. If the PokeAPI is down then the console app doesnt have much use anyway.

- HttpClient could also be updated with a retry policy but the same notes apply from error handling. Since the application is pretty quick, the user can just rerun the dotnet run command or if they are tech savvy manually check the PokeAPI to see if its up.

- This is a personal preference and moreso for programs deployed in environments or with more sensitive information. The RestAPI endpoint should be in an appconfig json file. Any security credentials should be environment variables and there would be relevant changes made to Program.Cs to configure the application.

- Frontend/UI way outside of the scope of the project but a nice frontend displaying some of the resource information including pokemon and some graphics for types would also be really cool.

https://json2csharp.com/ was a great resource used to plugin the api responses and generate models for the respective API results.
