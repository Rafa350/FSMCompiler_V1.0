using System;
using System.Collections.Generic;
using System.Linq;

namespace MikroPicDesigns.FSMCompiler.v1.Model {

    public sealed class Machine : IVisitable {

        private List<State> _stateList;
        private List<Variable> _variableList;
        private readonly string _name;
        private State _start;
        private Action _initializeAction;
        private Action _terminateAction;

        /// <summary>
        /// Constructior.
        /// </summary>
        /// <param name="name">El nom de la maquina.</param>
        /// 
        public Machine(string name) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            _name = name;
        }

        /// <summary>
        /// Accepta un visitador.
        /// </summary>
        /// <param name="visitor">El visitador.</param>
        /// 
        public void AcceptVisitor(IVisitor visitor) {

            if (visitor == null)
                throw new ArgumentNullException(nameof(visitor));

            visitor.Visit(this);
        }

        /// <summary>
        /// Afegeix un estat a la maquina.
        /// </summary>
        /// <param name="state">L'estat a afeigir.</param>
        /// 
        public void AddState(State state) {

            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (_stateList == null)
                _stateList = new List<State>();

            if (_stateList.Contains(state))
                throw new InvalidOperationException(
                    String.Format("El estado '{0}' ya ha sido agregado.", state.FullName));

            _stateList.Add(state);
        }

        /// <summary>
        /// Afegeix una coleccio d'estats a la maquina.
        /// </summary>
        /// <param name="states">Els estats a afeigir.</param>
        /// 
        public void AddStates(IEnumerable<State> states) {

            if (states == null)
                throw new ArgumentNullException(nameof(states));

            foreach (var state in states)
                AddState(state);
        }

        /// <summary>
        /// Obte un estat afeigit previament a la maquina.
        /// </summary>
        /// <param name="name">El nom del estat.</param>
        /// <param name="throwError">True si cal generar una excepcio en cas d'error.</param>
        /// <returns>L'estat.</returns>
        /// 
        public State GetState(string name, bool throwError = true) {

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (_stateList != null)
                foreach (State state in _stateList)
                    if (state.Name == name)
                        return state;

            if (throwError)
                throw new InvalidOperationException(
                    String.Format("No se agrego ningun estado con el nombre '{0}'.", name));

            return null;
        }

        /// <summary>
        /// Afegeix una variable.
        /// </summary>
        /// <param name="variable">La variable.</param>
        /// 
        public void AddVariable(Variable variable) {

            if (variable == null)
                throw new ArgumentNullException(nameof(variable));

            if (_variableList == null)
                _variableList = new List<Variable>();

            if (_variableList.Contains(variable))
                throw new InvalidOperationException(
                    String.Format("La variable '{0}' ya ha sido agregada.", variable.Name));

            _variableList.Add(variable);
        }

        /// <summary>
        /// Afegeixz una coleccio de variables.
        /// </summary>
        /// <param name="variables">Les variables.</param>
        /// 
        public void AddVariables(IEnumerable<Variable> variables) {

            if (variables == null)
                throw new ArgumentNullException(nameof(variables));

            foreach (var variable in variables)
                AddVariable(variable);
        }

        /// <summary>
        /// Obte el nom de la maquina.
        /// </summary>
        /// 
        public string Name => 
            _name;

        /// <summary>
        /// Obte l'estat inicial de la maquina.
        /// </summary>
        /// 
        public State Start {
            get {
                return _start;
            }
            set {
                if ((_stateList == null) || !_stateList.Contains(value)) {
                    throw new InvalidOperationException(
                        String.Format("El estado '{0}', no esta declarado en esta maquina.", value.Name));
                }

                _start = value;
            }
        }

        public Action InitializeAction {
            get => _initializeAction;
            set => _initializeAction = value;
        }

        public Action TerminateAction {
            get => _terminateAction;
            set => _terminateAction = value;
        }

        /// <summary>
        /// Enumera els noms dels estats de la maquina.
        /// </summary>
        /// 
        public IEnumerable<string> StateNames =>
            _stateList.Select(state => state.Name);

        /// <summary>
        /// Enumera els estats de la maquina.
        /// </summary>
        /// 
        public IEnumerable<State> States => 
            _stateList;

        public IEnumerable<State> FinalStates =>
            _stateList.Where(state => !state.HasChilds);

        /// <summary>
        /// Enumera les variables de la maquina.
        /// </summary>
        /// 
        public IEnumerable<Variable> Variables =>
            _variableList;

        /// <summary>
        /// Enumera els noms de les variables.
        /// </summary>
        /// 
        public IEnumerable<string> VariablesNames =>
            _variableList.Select(variable => variable.Name);

        public bool HasVariables =>
            (_variableList != null) && (_variableList.Count > 0);
    }
}
