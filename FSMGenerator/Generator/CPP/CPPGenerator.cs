﻿namespace MikroPicDesigns.FSMCompiler.v1.Generator.CPP {

    using System;
    using System.IO;
    using MikroPicDesigns.FSMCompiler.v1.Model;

    public sealed class CPPGenerator : GeneratorBase {

        private readonly CPPGeneratorOptions options;

        public CPPGenerator(GeneratorParameters generatorParameters) {

            CPPGeneratorOptions options = new CPPGeneratorOptions();
            generatorParameters.Populate(options);
            this.options = options;
        }

        public override void Generate(Machine machine) {

            GenerateMachineCode(machine);
        }


        /// <summary>
        /// \brief Genera el fitxer de codi de la maquina d'estat.
        /// </summary>
        /// <param name="machine">La maquina.</param>
        /// 
        private void GenerateMachineCode(Machine machine) {

            string folder = options.OutputPath;
            if (String.IsNullOrEmpty(folder))
                folder = @".\";

            // Crea el fitxer "fsm_<machine>.c"
            //
            string fileName = String.Format("fsm_{0}.c", machine.Name);
            string path = Path.Combine(folder, fileName);
            using (StreamWriter writer = File.CreateText(path)) {

                CodeBuilder codeBuilder = new CodeBuilder();

                codeBuilder
                    .WriteLine("// -----------------------------------------------------------------------")
                    .WriteLine("// Generated by FsmCompiler v1.1")
                    .WriteLine("// Finite state machine compiler tool")
                    .WriteLine("// Copyright 2015-2020 Rafael Serrano (rsr.openware@gmail.com)")
                    .WriteLine("//")
                    .WriteLine("// Warning. Don't touch. Changes will be overwritten!")
                    .WriteLine("//")
                    .WriteLine("// -----------------------------------------------------------------------")
                    .WriteLine()
                    .WriteLine("#include \"fsm.h\"")
                    .WriteLine("#include \"fsm_defs.h\"")
                    .WriteLine()
                    .WriteLine();

                CodeGenerator generator = new CodeGenerator(machine);

                // Genera el enumerador amb els estats de la maquina
                //
                generator.GenerateStateTypeDeclaration(codeBuilder);
                
                // Genera les Accions i les guardes
                //
                generator.GenerateActionImplementation(codeBuilder);
                generator.GenerateGuardImplementation(codeBuilder);

                // Genera el procesador de la maquina en moduls 'switch/case'
                //
                generator.GenerateProcessorImplementation(codeBuilder);

                writer.Write(codeBuilder.ToString());
            }
        }
    }
}