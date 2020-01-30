namespace MikroPicDesigns.FSMCompiler {

    using System;
    using MikroPicDesigns.FSMCompiler.CmdLine;
    using MikroPicDesigns.FSMCompiler.v1.Generator;
    using MikroPicDesigns.FSMCompiler.v1.Generator.C;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP;
    using MikroPicDesigns.FSMCompiler.v1.Generator.CPP2;
    using MikroPicDesigns.FSMCompiler.v1.Generator.DOT;
    using MikroPicDesigns.FSMCompiler.v1.Loader;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    class Program {

        static void Main(string[] args) {

            try {
                CmdLineParser cmdLineParser = new CmdLineParser("FSMCompiler v1.0");
                cmdLineParser.Add(new ArgumentDefinition("source", 1, "Archivo de entrada.", true));
                cmdLineParser.Add(new OptionDefinition("G", "Generador."));
                cmdLineParser.Add(new OptionDefinition("H", "Ayuda."));
                cmdLineParser.Add(new OptionDefinition("P", "Parametro especifico del generador."));

                if (args.Length == 0) {
                    Console.WriteLine(cmdLineParser.HelpText);
                    Console.ReadKey(true);
                }

                else {

                    string generatorName = "C";
                    string sourceFileName = "";
                    GeneratorParameters generatorParameters = new GeneratorParameters();

                    cmdLineParser.Parse(args);
                    foreach (OptionInfo optionInfo in cmdLineParser.Options) {
                        switch (optionInfo.Name) {
                            case "G":
                                generatorName = optionInfo.Value;
                                break;

                            case "P":
                                generatorParameters.Add(optionInfo.Value);
                                break;
                        }
                    }
                    foreach (ArgumentInfo argumentInfo in cmdLineParser.Arguments) {
                        switch (argumentInfo.Name) {
                            case "source":
                                sourceFileName = argumentInfo.Value;
                                break;
                        }
                    }

                    Console.WriteLine();
                    Console.WriteLine("FsmCompiler v1.1");
                    Console.WriteLine("-------------------------------------------------------------");
                    Console.WriteLine("  Finite state machine compiler tool.");
                    Console.WriteLine("  Copyright 2015-2018 Rafael Serrano (rsr.openware@gmail.com)");
                    Console.WriteLine();

                    XmlLoader loader = new XmlLoader();
                    Machine machine = loader.Load(sourceFileName);

                    IGenerator generator = null;
                    switch (generatorName) {
                        case "DOT":
                            generator = new DOTGenerator(generatorParameters);
                            break;

                        case "C":
                        default:
                            generator = new CGenerator(generatorParameters);
                            break;

                        case "CPP":
                            generator = new v1.Generator.CPP.CPPGenerator(generatorParameters);
                            break;

                        case "CPP2":
                            generator = new v1.Generator.CPP2.CPPGenerator(generatorParameters);
                            break;
                    }

                    generator.Generate(machine);
                }
            }

            catch (Exception e) {
                while (e != null) {
                    Console.WriteLine(e.Message);
                    e = e.InnerException;
                }
            }
        }
    }
}
