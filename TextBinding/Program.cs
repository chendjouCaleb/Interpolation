// See https://aka.ms/new-console-template for more information

using System;
using System.Threading.Channels;
using TextBinding;
using TextBinding.Operators;

//Tokenizer tokenizer = new Tokenizer("bonjour et bienvenue {{(1+2)*4-5%4+x+\"bonjour à tous\" + user.name.surname}}");
//Tokenizer tokenizer = new Tokenizer("bonjour et bienvenue {{ 1.1.e+2*4-5%4+(1+2*((1))) + cos().x(a+b, b).x + (user.x) }} et sot");
//Tokenizer tokenizer = new Tokenizer("bonjour et bienvenue {{ 3*-3}} et sot {{((1)+(2))}}");
Tokenizer tokenizer = new Tokenizer("bonjour et bienvenue {{true && false}}.");
tokenizer.Tokenize();
var tokens = tokenizer.Tokens;

foreach (Token token in tokens)
{
    int a = -3-+3;
    double bb = 01.2;

    //Console.WriteLine(token);
}



BindingItemsBuilder builder = new (tokenizer.Tokens.ToList());

builder.Build();
BindingItems bindingItems = builder.Items;
Console.WriteLine(bindingItems.Execute());
Console.WriteLine(3+3*+4*(1+1));

var factory = new OperatorFactory(new[] {typeof(RealOperatorOverload)});

// User user = new User
// {
//     FirstName = "Chendjou",
//     LastName = "Caleb",
//     Birthday = new DateTime(1996, 12, 6, 10, 10, 10)
// };
//
// Binding<User> binding = new ();
// binding.Value = user;
//
// binding.AddText("user", "Nom: {{FirstName}} {{LastSurname}}");
// binding.AddText("date", "Date de naissance: {{Birthday.Day}} Jours, {{Birthday.Hour}} Heures, et {{Birthday.Minute}}");
// binding.AddText("compute1", "1+2={{ 1+2 }}");
//binding.AddText("compute2", "1+2*3={{ 1+2 * 3 }}");
//
// binding.Compile();
//
// string name = binding.Get("user");
// Console.WriteLine(binding.Get("date"));
// Console.WriteLine(binding.Get("compute1"));
