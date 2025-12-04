using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Pokemon_API
{

    class Program
    {

        private const String baseUrl = "https://pokeapi.co/api/v2/";
        private const String getPokemon = "pokemon/";
        private const String getType = "type/";

        static HttpClient client = new HttpClient();

        static void ShowAttributes(PokemonAttributes attributes)
        {
            Console.WriteLine($"Name: {attributes.name}\t");
        }

        static void ShowDamageRelations(DamageRelations damageRelations)
        {

            List<String> strongAgainst = new List<String>();
            List<String> weakAgainst = new List<String>();


            UpdateStrengthList(weakAgainst, damageRelations.half_damage_to);
            UpdateStrengthList(weakAgainst, damageRelations.double_damage_from);
            UpdateStrengthList(weakAgainst, damageRelations.no_damage_to);

            UpdateStrengthList(strongAgainst, damageRelations.half_damage_from);
            UpdateStrengthList(strongAgainst, damageRelations.double_damage_to);
            UpdateStrengthList(strongAgainst, damageRelations.no_damage_from);


            Console.WriteLine("Weak Against:");
            ShowStrengthList(weakAgainst.Distinct().ToList());

            Console.WriteLine();

            Console.WriteLine("Strong Against:");
            ShowStrengthList(strongAgainst.Distinct().ToList());

        }

        static void ShowStrengthList(List<String> strengthList)
        {
            foreach (var name in strengthList) { Console.Write($"{name}\t"); }

        }

        static void UpdateStrengthList(List<String> strengthList, List<ApiResource> strengthTypeList)
        {
            foreach (var typeName in strengthTypeList)
            { strengthList.Add(typeName.name); }
        }

        static async Task<PokemonAttributes> GetAttributesAsync(string pokemon)
        {
            PokemonAttributes pokemonAttributes = null;
            HttpResponseMessage response = await client.GetAsync(baseUrl + getPokemon + pokemon);

            if (response.IsSuccessStatusCode)
            {
                pokemonAttributes = await response.Content.ReadAsAsync<PokemonAttributes>();
            }
            return pokemonAttributes;
        }

        static async Task<PokemonTypeInfo> GetTypeAsync(String type)
        {
            PokemonTypeInfo pokemonType = null;
            HttpResponseMessage response = await client.GetAsync(baseUrl + getType + type.ToString());

            if (response.IsSuccessStatusCode)
            {
                pokemonType = await response.Content.ReadAsAsync<PokemonTypeInfo>();
            }
            return pokemonType;
        }

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {

                Console.WriteLine("Please Enter a Pokemon:");
                String userPokemon = Console.ReadLine();
                if (string.IsNullOrEmpty(userPokemon))
                {
                    Console.WriteLine("Please Enter a Valid Pokemon Name");
                }
                // Get the product
                PokemonAttributes pokemonAttributes = await GetAttributesAsync(userPokemon);
                ShowAttributes(pokemonAttributes);

                List<PokemonType> pokemonTypes = pokemonAttributes.types;
                foreach (var pokemonType in pokemonTypes)
                {
                    PokemonTypeInfo typeInfo = await GetTypeAsync(pokemonType.type.name);

                    Console.WriteLine($"Type: {typeInfo.name}\t");
                    ShowDamageRelations(typeInfo.damage_relations);
                }


                // // Update the product
                // Console.WriteLine("Updating price...");
                // product.Price = 80;
                // await UpdateProductAsync(product);

                // // Get the updated product
                // product = await GetProductAsync(url.PathAndQuery);
                // ShowProduct(product);

                // // Delete the product
                // var statusCode = await DeleteProductAsync(product.Id);
                // Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }


}



