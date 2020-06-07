using System;
using System.Collections.Generic;

namespace Calculator.Models
{
    public enum States { State0 = 0, State1, State2, State3, State4, State5 };

    public class CalculatorEngine
    {
        private char operatorInWaiting;

        private double currentOperand;
        private double fractionalDivisor;

        private double accumulator;
        private double result;

        private States state;
        private IDictionary<char, StateAction> state0;
        private IDictionary<char, StateAction> state1;
        private IDictionary<char, StateAction> state2;
        private IDictionary<char, StateAction> state3;
        private IDictionary<char, StateAction> state4;
        private IDictionary<char, StateAction> state5;

        private IDictionary<States, IDictionary<char, StateAction>> stateSet;

        private IDictionary<char, Func<double, double, double>> OperatorFunctions;



        public CalculatorEngine()
        {
            // Start of Calculation 
            state0 = new Dictionary<char, StateAction>
            {
                { '0', new StateAction(StartOfNewCalculation, States.State1) },
                { '1', new StateAction(StartOfNewCalculation, States.State1) },
                { '2', new StateAction(StartOfNewCalculation, States.State1) },
                { '3', new StateAction(StartOfNewCalculation, States.State1) },
                { '4', new StateAction(StartOfNewCalculation, States.State1) },
                { '5', new StateAction(StartOfNewCalculation, States.State1) },
                { '6', new StateAction(StartOfNewCalculation, States.State1) },
                { '7', new StateAction(StartOfNewCalculation, States.State1) },
                { '8', new StateAction(StartOfNewCalculation, States.State1) },
                { '9', new StateAction(StartOfNewCalculation, States.State1) },
                { '.', new StateAction(StartOfNewCalculation, States.State1) },
                { '+', new StateAction(Operator, States.State3) },
                { '-', new StateAction(Operator, States.State3) },
                { '*', new StateAction(Operator, States.State3) },
                { '/', new StateAction(Operator, States.State3) },
                { '=', new StateAction(Result, States.State0) },
                { 'C', new StateAction(Clear, States.State0) }
            };

            // Defining integral part of first operand State
            state1 = new Dictionary<char, StateAction>
            {
                { '0', new StateAction(FirstIntegralDigit, States.State1) },
                { '1', new StateAction(FirstIntegralDigit, States.State1) },
                { '2', new StateAction(FirstIntegralDigit, States.State1) },
                { '3', new StateAction(FirstIntegralDigit, States.State1) },
                { '4', new StateAction(FirstIntegralDigit, States.State1) },
                { '5', new StateAction(FirstIntegralDigit, States.State1) },
                { '6', new StateAction(FirstIntegralDigit, States.State1) },
                { '7', new StateAction(FirstIntegralDigit, States.State1) },
                { '8', new StateAction(FirstIntegralDigit, States.State1) },
                { '9', new StateAction(FirstIntegralDigit, States.State1) },
                { '.', new StateAction(DecimalPoint, States.State2) },
                { '+', new StateAction(Operator, States.State3) },
                { '-', new StateAction(Operator, States.State3) },
                { '*', new StateAction(Operator, States.State3) },
                { '/', new StateAction(Operator, States.State3) },
                { '=', new StateAction(Result, States.State0) },
                { 'C', new StateAction(Clear, States.State0) }
            };

            // Defining decimal part of first operand
            state2 = new Dictionary<char, StateAction>
            {
                { '0', new StateAction(FirstFractionalDigit, States.State2) },
                { '1', new StateAction(FirstFractionalDigit, States.State2) },
                { '2', new StateAction(FirstFractionalDigit, States.State2) },
                { '3', new StateAction(FirstFractionalDigit, States.State2) },
                { '4', new StateAction(FirstFractionalDigit, States.State2) },
                { '5', new StateAction(FirstFractionalDigit, States.State2) },
                { '6', new StateAction(FirstFractionalDigit, States.State2) },
                { '7', new StateAction(FirstFractionalDigit, States.State2) },
                { '8', new StateAction(FirstFractionalDigit, States.State2) },
                { '9', new StateAction(FirstFractionalDigit, States.State2) },
                { '.', new StateAction(DecimalPointNotAllowed, States.State0) },
                { '+', new StateAction(Operator, States.State3) },
                { '-', new StateAction(Operator, States.State3) },
                { '*', new StateAction(Operator, States.State3) },
                { '/', new StateAction(Operator, States.State3) },
                { '=', new StateAction(Result, States.State0) },
                { 'C', new StateAction(Clear, States.State0) }
            };

            // Defining first integral number of second operand
            state3 = new Dictionary<char, StateAction>
            {
                { '0', new StateAction(IntegralDigit, States.State4) },
                { '1', new StateAction(IntegralDigit, States.State4) },
                { '2', new StateAction(IntegralDigit, States.State4) },
                { '3', new StateAction(IntegralDigit, States.State4) },
                { '4', new StateAction(IntegralDigit, States.State4) },
                { '5', new StateAction(IntegralDigit, States.State4) },
                { '6', new StateAction(IntegralDigit, States.State4) },
                { '7', new StateAction(IntegralDigit, States.State4) },
                { '8', new StateAction(IntegralDigit, States.State4) },
                { '9', new StateAction(IntegralDigit, States.State4) },
                { '.', new StateAction(DecimalPoint, States.State5) },
                { '+', new StateAction(OperatorNotAllowed, States.State0) },
                { '-', new StateAction(OperatorNotAllowed, States.State0) },
                { '*', new StateAction(OperatorNotAllowed, States.State0) },
                { '/', new StateAction(OperatorNotAllowed, States.State0) },
                { '=', new StateAction(Result, States.State0) },
                { 'C', new StateAction(Clear, States.State0) }
            };

            // Defining integral part of second operand
            state4 = new Dictionary<char, StateAction>
            {
                { '0', new StateAction(IntegralDigit, States.State4) },
                { '1', new StateAction(IntegralDigit, States.State4) },
                { '2', new StateAction(IntegralDigit, States.State4) },
                { '3', new StateAction(IntegralDigit, States.State4) },
                { '4', new StateAction(IntegralDigit, States.State4) },
                { '5', new StateAction(IntegralDigit, States.State4) },
                { '6', new StateAction(IntegralDigit, States.State4) },
                { '7', new StateAction(IntegralDigit, States.State4) },
                { '8', new StateAction(IntegralDigit, States.State4) },
                { '9', new StateAction(IntegralDigit, States.State4) },
                { '.', new StateAction(DecimalPoint, States.State5) },
                { '+', new StateAction(CalcAndOperator, States.State3) },
                { '-', new StateAction(CalcAndOperator, States.State3) },
                { '*', new StateAction(CalcAndOperator, States.State3) },
                { '/', new StateAction(CalcAndOperator, States.State3) },
                { '=', new StateAction(CalcAndResult, States.State0) },
                { 'C', new StateAction(Clear, States.State0) }
            };


            // Defining decimal part of subsequent operand.
            state5 = new Dictionary<char, StateAction>
            {
                { '0', new StateAction(FractionalDigit, States.State5) },
                { '1', new StateAction(FractionalDigit, States.State5) },
                { '2', new StateAction(FractionalDigit, States.State5) },
                { '3', new StateAction(FractionalDigit, States.State5) },
                { '4', new StateAction(FractionalDigit, States.State5) },
                { '5', new StateAction(FractionalDigit, States.State5) },
                { '6', new StateAction(FractionalDigit, States.State5) },
                { '7', new StateAction(FractionalDigit, States.State5) },
                { '8', new StateAction(FractionalDigit, States.State5) },
                { '9', new StateAction(FractionalDigit, States.State5) },
                { '.', new StateAction(DecimalPointNotAllowed, States.State0) },
                { '+', new StateAction(CalcAndOperator, States.State3) },
                { '-', new StateAction(CalcAndOperator, States.State3) },
                { '*', new StateAction(CalcAndOperator, States.State3) },
                { '/', new StateAction(CalcAndOperator, States.State3) },
                { '=', new StateAction(CalcAndResult, States.State0) },
                { 'C', new StateAction(Clear, States.State0) }
            };

            OperatorFunctions = new Dictionary<char, Func<double, double, double>> {
            { '+', PlusOperator},
            { '-', MinusOperator },
            { '*', MultiplicationOperator },
            { '/', DivisionOperator } };

            stateSet = new Dictionary<States, IDictionary<char, StateAction>>()
            {
                {States.State0, state0 },
                {States.State1, state1 },
                {States.State2, state2 },
                {States.State3, state3 },
                {States.State4, state4 },
                {States.State5, state5 }
            };

            //
            ResetCalculator();
        }

        public double Calculate(char key)
        {
            stateSet[state][key].Action(key);
            state = stateSet[state][key].TransitionToState;
            return result;
        }

        #region Key Functions
        private void StartOfNewCalculation(char key)
        {
            accumulator = 0.0;
            result = accumulator;
            state = stateSet[state][key].TransitionToState;
            stateSet[state][key].Action(key);
        }

        private void FirstIntegralDigit(char key)
        {
            accumulator = accumulator * 10.0 + Char.GetNumericValue(key);
            result = accumulator;
        }

        private void DecimalPoint(char key)
        {
            // No action required. Just a change of state
        }


        private void FirstFractionalDigit(char key)
        {
            fractionalDivisor *= 10.0;
            accumulator = accumulator + (Char.GetNumericValue(key) / fractionalDivisor);
            result = accumulator;
        }

        private void Operator(char key)
        {
            ResetNumerics();
            operatorInWaiting = key;
        }

        private void IntegralDigit(char key)
        {

            currentOperand = currentOperand * 10.0 + Char.GetNumericValue(key);
            result = currentOperand;
        }

        private void FractionalDigit(char key)
        {
            fractionalDivisor *= 10.0;
            currentOperand = currentOperand + (Char.GetNumericValue(key) / fractionalDivisor);
            result = currentOperand;
        }

        private void CalcAndOperator(char key)
        {
            ExecuteStackedOperator();
            Operator(key);
        }

        private void DecimalPointNotAllowed(char key)
        {
            ResetCalculator();
            throw new Exception("Error: Decimal Point not valid");
        }

        private void OperatorNotAllowed(char key)
        {
            ResetCalculator();
            throw new Exception("Error: Operator not valid");
        }

        private void Result(char key)
        {
            ResetNumerics();
        }

        private void CalcAndResult(char key)
        {
            ExecuteStackedOperator();
            ResetNumerics();
        }

        private void Clear(char key)
        {
            ResetCalculator();
        }

        #endregion Key Functions
        #region Operator Functions
        private double PlusOperator(double operand1, double operand2)
        {
            return operand1 + operand2;
        }
        private double MinusOperator(double operand1, double operand2)
        {
            return operand1 - operand2; ;
        }
        private double MultiplicationOperator(double operand1, double operand2)
        {
            return operand1 * operand2;
        }
        private double DivisionOperator(double operand1, double operand2)
        {
            return operand1 / operand2;
        }
        #endregion Operator Functions

        #region State transition and Operator execution
        private void TransitionState(States requiredState)
        {
            state = requiredState;
        }

        private void ExecuteStackedOperator()
        {
            accumulator = OperatorFunctions[operatorInWaiting](accumulator, currentOperand);
            result = accumulator;
        }
        #endregion State transition and Operator execution

        private void ResetCalculator()
        {
            state = 0;
            accumulator = 0.0;
            result = accumulator;
            ResetNumerics();
            TransitionState(States.State0);
        }

        private void ResetNumerics()
        {
            currentOperand = 0.0;
            fractionalDivisor = 1.0;
        }
    }
}

