using System;
using static System.Console;

namespace first {
    class Program {
    static void Main(){
        // Write("Enter your name: ");
        // string name = ReadLine();
        // WriteLine($"Welcome {name}!");
        Write("Enter 2 numbers: ");
        string num1 = ReadLine();
        string num2 = ReadLine();
        Write("Enter operator: ");
        string op = ReadLine();

        // if(op == "+"){
        //     WriteLine(int.Parse(num1) + int.Parse(num2));
        // }else if(op == "-"){
        //     WriteLine(int.Parse(num1) - int.Parse(num2));
        // }else if(op == "*"){
        //     WriteLine(int.Parse(num1) * int.Parse(num2));
        // }else if(op == "/"){
        //     WriteLine(int.Parse(num1) / int.Parse(num2));
        // }else {
        //     WriteLine(int.Parse(num1) % int.Parse(num2));
        // }

        switch(op){
            case "+":
                WriteLine(int.Parse(num1) + int.Parse(num2));
                break;
            case "-":
                WriteLine(int.Parse(num1) - int.Parse(num2));
                break;
            case "*":
                WriteLine(int.Parse(num1) * int.Parse(num2));
                break;
            case "/":
                WriteLine(int.Parse(num1) / int.Parse(num2));
                break;
            case "%":
                WriteLine(int.Parse(num1) % int.Parse(num2));
                break;
        }
    }
}

}

