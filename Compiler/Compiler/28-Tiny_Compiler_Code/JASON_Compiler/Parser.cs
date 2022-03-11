using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;
        Boolean mainFunctionIsPresent = false;
        public Node StartParsing(List<Token> TokenStream)
        {
            this.TokenStream = TokenStream;
            root = new Node("Root Node");
            root.Children.Add(Program());

            if (!mainFunctionIsPresent)
                Errors.Error_List.Add("Your code doesn't contain a main()\n");

            return root;
        }
        Node Program() // Start Predection Rule
        {
            Node program = new Node("Program");
            program.Children.Add(Function_Statements());
            return program;
        }
        Node Function_Statements()
        {
            if (CheckForNull(Token_Class.Integer) || CheckForNull(Token_Class.String) || CheckForNull(Token_Class.Float))
            {
                Node functionStatements = new Node("Function_Statements");
                functionStatements.Children.Add(Function_Statement());
                functionStatements.Children.Add(Function_Statements());
                return functionStatements;
            }
            else if (CheckForNull(Token_Class.Comment))
            {
                Node functionStatements = new Node("Comment");
                functionStatements.Children.Add(Match(Token_Class.Comment));
                functionStatements.Children.Add(Function_Statements());
                return functionStatements;
            }
            return null;
        }
        Node Function_Statement()
        {
            Node functionStatement = new Node("Function_Statement");
            functionStatement.Children.Add(Function_Declaration());
            functionStatement.Children.Add(Function_Body());
            return functionStatement;
        }
        Node Function_Declaration()
        {
            Node functionDeclaration = new Node("Function_Declaration");
            functionDeclaration.Children.Add(Datatype());
            functionDeclaration.Children.Add(Function_Declaration_());
            return functionDeclaration;
        }
        Node Function_Declaration_()
        {
            Node functionDeclaration_ = new Node("Function_Declaration_");
            if (CheckForNull(Token_Class.Main))
            {
                functionDeclaration_.Children.Add(Main_Function());
            }
            else
            {
                functionDeclaration_.Children.Add(FunctionName());
                functionDeclaration_.Children.Add(Match(Token_Class.LParanthesis));
                functionDeclaration_.Children.Add(Parameters());
                functionDeclaration_.Children.Add(Match(Token_Class.RParanthesis));
            }
            return functionDeclaration_;
        }
        Node Function_Body()
        {
            Node functionBody = new Node("Function_Body");
            functionBody.Children.Add(Match(Token_Class.LBrace));
            functionBody.Children.Add(Statements());
            functionBody.Children.Add(Return_Statement());
            functionBody.Children.Add(Match(Token_Class.RBrace));
            return functionBody;
        }
        Node FunctionName()
        {
            Node functionName = new Node("FunctionName");
            functionName.Children.Add(Match(Token_Class.Idenifier));
            return functionName;
        }
        Node Main_Function() // main()
        {
            mainFunctionIsPresent = true;
            Node mainFunction = new Node("Main_Function");
            //mainFunction.Children.Add(Datatype());
            mainFunction.Children.Add(Match(Token_Class.Main));
            mainFunction.Children.Add(Match(Token_Class.LParanthesis));
            mainFunction.Children.Add(Match(Token_Class.RParanthesis));
            //mainFunction.Children.Add(Function_Body());
            return mainFunction;
        }
        Node Parameters()
        {
            if (CheckForNull(Token_Class.Integer) || CheckForNull(Token_Class.String) || CheckForNull(Token_Class.Float))
            {
                Node parameters = new Node("Parameters");
                parameters.Children.Add(Parameter());
                parameters.Children.Add(Parameters_());
                return parameters;
            }
            return null;
        }
        Node Parameters_()
        {
            if (CheckForNull(Token_Class.Comma))
            {
                Node parameters_ = new Node("Parameters_");
                parameters_.Children.Add(Match(Token_Class.Comma));
                parameters_.Children.Add(Parameters());
                return parameters_;
            }
            return null;
        }
        Node Parameter()
        {
            Node parameter = new Node("Parameter");
            parameter.Children.Add(Datatype());
            parameter.Children.Add(Match(Token_Class.Idenifier));
            return parameter;
        }

        Node Statements()
        {
            //Statements ➔ Statement Statements | ε
            if (CheckForNull(Token_Class.Idenifier) || CheckForNull(Token_Class.String) ||
                CheckForNull(Token_Class.Integer) || CheckForNull(Token_Class.Float)
                || CheckForNull(Token_Class.If) || CheckForNull(Token_Class.Repeat)
                || CheckForNull(Token_Class.Read) || CheckForNull(Token_Class.Write)
                || CheckForNull(Token_Class.Comment))
            {
                Node statements = new Node("Statements");
                statements.Children.Add(Statement());
                statements.Children.Add(Statements());
                return statements;
            }
            return null;
        }
        Node Statement()
        {
            // Statement ➔ EndsWithSemicolon ; | NoSemicolon
            Node statement = new Node("Statement");
            if (CheckForNull(Token_Class.If) || CheckForNull(Token_Class.Repeat)
                || CheckForNull(Token_Class.Comment))
            {
                statement.Children.Add(No_Semicolon());
            }
            else
            {
                statement.Children.Add(Ends_With_Semicolon());
                statement.Children.Add(Match(Token_Class.Semicolon));
            }
            return statement;
        }
        Node Ends_With_Semicolon()
        {
            // EndsWithSemicolon ➔ Assignment_Statement | Declaration_Statement | Write_Statement | Read_Statement | Function_Call
            Node ends_with_semicolon = new Node("Ends_with_semicolon");
            if (CheckForNull(Token_Class.Idenifier))
            {
                ends_with_semicolon.Children.Add(Match(Token_Class.Idenifier));
                ends_with_semicolon.Children.Add(EndsWithSemi_());
            }
            else if (CheckForNull(Token_Class.Integer) || CheckForNull(Token_Class.Float) || CheckForNull(Token_Class.String))
                ends_with_semicolon.Children.Add(Declaration_Statement());
            else if (CheckForNull(Token_Class.Write))
                ends_with_semicolon.Children.Add(Write_Statement());
            else if (CheckForNull(Token_Class.Read))
                ends_with_semicolon.Children.Add(Read_Statement());
            return ends_with_semicolon;
        }
        Node EndsWithSemi_()
        {
            Node endsWithSemi = new Node("EndsWithSemi_");
            if (CheckForNull(Token_Class.LParanthesis))
            {
                endsWithSemi.Children.Add(Parameters_Part());
            }
            else
            {
                endsWithSemi.Children.Add(Assignment_Statement());
            }
            return endsWithSemi;
        }
        Node No_Semicolon()
        {
            // NoSemicolon ➔ Repeat_Statement | If_Statement | Comment_Statement
            Node no_semicolon = new Node("No_Semicolon");
            if (CheckForNull(Token_Class.If))
                no_semicolon.Children.Add(If_Statement());
            else if (CheckForNull(Token_Class.Repeat))
                no_semicolon.Children.Add(Repeat_Statement());
            else
                no_semicolon.Children.Add(Match(Token_Class.Comment));
            return no_semicolon;
        }
        Node Return_Statement()
        {
            // Return_Statement ➔ return Expression ;
            Node return_statement = new Node("return_statement");
            return_statement.Children.Add(Match(Token_Class.Return));
            return_statement.Children.Add(Expression());
            return_statement.Children.Add(Match(Token_Class.Semicolon));
            return return_statement;
        }
        Node Function_Call()
        {
            // Function_Call ➔ identifier ParametersPart
            Node function_call = new Node("function_call");
            function_call.Children.Add(Match(Token_Class.Idenifier));
            function_call.Children.Add(Parameters_Part());
            return function_call;
        }
        Node Parameters_Part()
        {
            // ParametersPart  ➔ ( Identifiers ) 
            Node parameters_part = new Node("Parameters_part");
            parameters_part.Children.Add(Match(Token_Class.LParanthesis));
            parameters_part.Children.Add(Identifiers());
            parameters_part.Children.Add(Match(Token_Class.RParanthesis));
            return parameters_part;
        }
        Node Identifiers()
        {
            // Identifiers ➔ identifier Identifiers_ | ε
            Node identifiers = new Node("identifiers");
            if (CheckForNull(Token_Class.Idenifier))
            {
                identifiers.Children.Add(Match(Token_Class.Idenifier));
                identifiers.Children.Add(Identifiers_());
                return identifiers;
            }
            else if (CheckForNull(Token_Class.Constant))
            {
                identifiers.Children.Add(Match(Token_Class.Constant));
                identifiers.Children.Add(Identifiers_());
                return identifiers;
            }
            return null;
        }
        Node Identifiers_()
        {
            // Identifiers_ ➔ , Identifiers | ε
            Node identifiers_ = new Node("identifiers_");
            if (CheckForNull(Token_Class.Comma))
            {
                identifiers_.Children.Add(Match(Token_Class.Comma));
                identifiers_.Children.Add(Identifiers());
                return identifiers_;
            }
            return null;
        }
        Node Repeat_Statement()
        {
            // Repeat_Statement ➔ repeat Statements until Condition_Statement
            Node repeat_statement = new Node("Repeat_Statement");
            repeat_statement.Children.Add(Match(Token_Class.Repeat));
            repeat_statement.Children.Add(Statements());
            repeat_statement.Children.Add(Match(Token_Class.Until));
            repeat_statement.Children.Add(Condition_Statement());
            return repeat_statement;
        }
        Node Condition_Statement()
        {
            // Condition_Statement ➔ Condition Condition_Statement_ 
            Node condition_statement = new Node("Condition_Statement");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Condition_Statement_());
            return condition_statement;
        }
        Node Condition_Statement_()
        {
            // Condition_Statement_ ➔ Boolean_Operator Condition_Statement |  ε
            if (CheckForNull(Token_Class.Or) || CheckForNull(Token_Class.And))
            {
                Node condition_statement_ = new Node("Condition_Statement_");
                condition_statement_.Children.Add(Boolean_Operator());
                condition_statement_.Children.Add(Condition_Statement());
                return condition_statement_;
            }
            return null;
        }
        Node Condition()
        {
            // Condition ➔ identifier Condition_Operator Term
            Node condition = new Node("Condition");
            condition.Children.Add(Match(Token_Class.Idenifier));
            condition.Children.Add(Condition_Operator());
            condition.Children.Add(Term());
            return condition;
        }
        Node Term()
        {
            // Term ➔ number | identifier TermFactoring
            Node term = new Node("Term");
            if (CheckForNull(Token_Class.Idenifier))
            {
                term.Children.Add(Match(Token_Class.Idenifier));
                term.Children.Add(Term_Factoring());
            }
            else
                term.Children.Add(Match(Token_Class.Constant));
            return term;
        }
        Node Term_Factoring()
        {
            // TermFactoring ➔ ε | ParametersPart 
            if (CheckForNull(Token_Class.LParanthesis))
            {
                Node term_factoring = new Node("Term_Factoring");
                term_factoring.Children.Add(Parameters_Part());
                return term_factoring;
            }
            return null;
        }
        Node Boolean_Operator()
        {
            // Boolean_Operator ➔ && | “||”
            Node boolean_operator = new Node("Boolean_Operator");
            if (CheckForNull(Token_Class.Or))
            {
                boolean_operator.Children.Add(Match(Token_Class.Or));
            }
            else
            {
                boolean_operator.Children.Add(Match(Token_Class.And));
            }
            return boolean_operator;
        }
        Node Condition_Operator()
        {
            // Condition_Operator ➔ > | < | <> | =
            Node condition_operator = new Node("Condition_Operator");
            if (CheckForNull(Token_Class.EqualOp))
                condition_operator.Children.Add(Match(Token_Class.EqualOp));
            else if (CheckForNull(Token_Class.GreaterThanOp))
                condition_operator.Children.Add(Match(Token_Class.GreaterThanOp));
            else if (CheckForNull(Token_Class.LessThanOp))
                condition_operator.Children.Add(Match(Token_Class.LessThanOp));
            else
                condition_operator.Children.Add(Match(Token_Class.NotEqualOp));
            return condition_operator;
        }
        Node Write_Statement()
        {
            // Write_Statement ➔ write Next
            Node write_statement = new Node("Write_Statement");
            write_statement.Children.Add(Match(Token_Class.Write));
            write_statement.Children.Add(Next());
            return write_statement;
        }
        Node Next()
        {
            // Next ➔ Expression | endl
            Node next = new Node("Next");
            if (CheckForNull(Token_Class.Endl))
                next.Children.Add(Match(Token_Class.Endl));
            else
                next.Children.Add(Expression());
            return next;
        }
        Node Read_Statement()
        {
            // Read_Statement ➔ read identifier
            Node read_statement = new Node("Read_Statement");
            read_statement.Children.Add(Match(Token_Class.Read));
            read_statement.Children.Add(Match(Token_Class.Idenifier));
            return read_statement;
        }
        Node Datatype()
        {
            // Datatype ➔ int | float | string
            Node datatype = new Node("Datatype");
            if (CheckForNull(Token_Class.Integer))
                datatype.Children.Add(Match(Token_Class.Integer));
            else if (CheckForNull(Token_Class.Float))
                datatype.Children.Add(Match(Token_Class.Float));
            else if (CheckForNull(Token_Class.String))
                datatype.Children.Add(Match(Token_Class.String));
            return datatype;
        }
        Node Declaration_Statement()
        {
            // Declaration_Statement ➔ Datatype DeclarationDetails
            Node declaration_statement = new Node("Declaration_Statement");
            declaration_statement.Children.Add(Datatype());
            declaration_statement.Children.Add(Declaration_Details());
            return declaration_statement;
        }
        Node Declaration_Details()
        {
            // DeclarationDetails ➔ DeclarationDetail DeclarationDetails_
            Node declaration_details = new Node("Declaration_Details");
            declaration_details.Children.Add(Declaration_Detail());
            declaration_details.Children.Add(Declaration_Details_());
            return declaration_details;
        }
        Node Declaration_Details_()
        {
            // DeclarationDetails_ ➔ , DeclarationDetails | ε
            if (CheckForNull(Token_Class.Comma))
            {
                Node declaration_details_ = new Node("Declaration_Details_");
                declaration_details_.Children.Add(Match(Token_Class.Comma));
                declaration_details_.Children.Add(Declaration_Details());
                return declaration_details_;
            }
            return null;
        }
        Node Declaration_Detail()
        {
            // DeclarationDetail ➔ identifier OtherDetail 
            Node declaration_detail = new Node("Declaration_Detail");
            declaration_detail.Children.Add(Match(Token_Class.Idenifier));
            declaration_detail.Children.Add(Other_Detail());
            return declaration_detail;
        }
        Node Other_Detail()
        {
            // OtherDetail ➔ := Expression | ε
            if (CheckForNull(Token_Class.Assignment))
            {
                Node other_detail = new Node("Other_Detail");
                other_detail.Children.Add(Assignment_Statement());
                return other_detail;
            }
            return null;
        }

        Node Assignment_Statement()
        {
            //Assignment_Statement ➔ := Expression
            Node assignmentStatement = new Node("Assignment_Statement");
            //assignmentStatement.Children.Add(Match(Token_Class.Identifier));
            assignmentStatement.Children.Add(Match(Token_Class.Assignment));
            assignmentStatement.Children.Add(Expression());
            return assignmentStatement;
        }
        Node Expression()
        {
            //Expression ➔ String | Term ExtraExpression | (Equation)
            Node expression = new Node("Expression");
            if (CheckForNull(Token_Class.String))
                expression.Children.Add(Match(Token_Class.String));
            else if (CheckForNull(Token_Class.LParanthesis))
            {

                expression.Children.Add(Match(Token_Class.LParanthesis));
                expression.Children.Add(Equation());
                expression.Children.Add(Match(Token_Class.RParanthesis));
            }
            else
            {
                expression.Children.Add(Term());
                expression.Children.Add(ExtraExpression());
            }
            return expression;
        }
        Node ExtraExpression()
        {
            if (CheckForNull(Token_Class.PlusOp) || CheckForNull(Token_Class.MinusOp) ||
                CheckForNull(Token_Class.MultiplyOp) || CheckForNull(Token_Class.DivideOp))
            {
                Node extraExpression = new Node("ExtraExpression");
                extraExpression.Children.Add(Arithmatic_Operator());
                extraExpression.Children.Add(Equation());
                return extraExpression;
            }
            return null;
        }
        /*
        Node Expression_()
        {
            Node expression_ = new Node("Expression_");
            if (CheckForNull(Token_Class.LParanthesis))
            {
                expression_.Children.Add(Match(Token_Class.LParanthesis));
                expression_.Children.Add(Equation());
                expression_.Children.Add(Match(Token_Class.RParanthesis));
            }
            else
                expression_.Children.Add(Equation());
            return expression_;
        }
        */
        Node Arithmatic_Operator()
        {
            Node arithmaticOperator = new Node("Arithmatic_Operator");
            if (CheckForNull(Token_Class.PlusOp))
                arithmaticOperator.Children.Add(Match(Token_Class.PlusOp));
            else if (CheckForNull(Token_Class.MinusOp))
                arithmaticOperator.Children.Add(Match(Token_Class.MinusOp));
            else if (CheckForNull(Token_Class.DivideOp))
                arithmaticOperator.Children.Add(Match(Token_Class.DivideOp));
            else if (CheckForNull(Token_Class.MultiplyOp))
                arithmaticOperator.Children.Add(Match(Token_Class.MultiplyOp));
            return arithmaticOperator;
        }
        Node Equation()
        {
            //Equation ➔ TermEq Equation_
            Node equation = new Node("equation");
            equation.Children.Add(TermEq());
            equation.Children.Add(Equation_());
            return equation;
        }
        Node Equation_()
        {
            //Equation_ ➔ AddTerm Equation_ | ε
            Node equation_ = new Node("equation_");
            if (CheckForNull(Token_Class.PlusOp) || CheckForNull(Token_Class.MinusOp))
            {
                equation_.Children.Add(AddTerm());
                equation_.Children.Add(Equation_());
                return equation_;
            }
            return null;
        }
        Node AddTerm()
        {
            //AddTerm ➔ AddOperation TermEq
            Node addTerm = new Node("AddTerm");
            addTerm.Children.Add(AddOperation());
            addTerm.Children.Add(TermEq());
            return addTerm;
        }
        Node TermEq()
        {
            //TermEq ➔ Factor TermEq_ 
            Node termEq = new Node("TermEq");
            termEq.Children.Add(Factor());
            termEq.Children.Add(TermEq_());
            return termEq;
        }
        Node TermEq_()
        {
            //TermEq_ ➔ MulTerm TermEq_ | ε
            Node termEq_ = new Node("TermEq_");
            if (CheckForNull(Token_Class.MultiplyOp) || CheckForNull(Token_Class.DivideOp))
            {
                termEq_.Children.Add(MulTerm());
                termEq_.Children.Add(TermEq_());
                return termEq_;
            }
            return null;
        }
        Node MulTerm()
        {
            //MulTerm ➔ MulOperation Factor
            Node mulTerm = new Node("MulTerm");
            mulTerm.Children.Add(MulOperation());
            mulTerm.Children.Add(Factor());
            return mulTerm;
        }
        Node Factor()
        {
            //Factor ➔ (Equation) | Term
            Node factor = new Node("Factor");
            if (CheckForNull(Token_Class.LParanthesis))
            {
                factor.Children.Add(Match(Token_Class.LParanthesis));
                factor.Children.Add(Equation());
                factor.Children.Add(Match(Token_Class.RParanthesis));
            }
            else
            {
                factor.Children.Add(Term());
            }
            return factor;
        }
        Node AddOperation()
        {
            //AddOperation ➔ + | -
            Node addOperation = new Node("addOperation");
            if (CheckForNull(Token_Class.PlusOp))
                addOperation.Children.Add(Match(Token_Class.PlusOp));
            else
                addOperation.Children.Add(Match(Token_Class.MinusOp));
            return addOperation;
        }
        Node MulOperation()
        {
            //MulOperation ➔ * | /
            Node mulOperation = new Node("mulOperation");
            if (CheckForNull(Token_Class.MultiplyOp))
                mulOperation.Children.Add(Match(Token_Class.MultiplyOp));
            else
                mulOperation.Children.Add(Match(Token_Class.DivideOp));
            return mulOperation;
        }
        Node If_Statement()
        {
            //If_Statement ➔ if Condition_Statement then Statements ElseClause
            Node ifStatement = new Node("If_Statement");
            ifStatement.Children.Add(Match(Token_Class.If));
            ifStatement.Children.Add(Condition_Statement());
            ifStatement.Children.Add(Match(Token_Class.Then));
            ifStatement.Children.Add(Statements());
            ifStatement.Children.Add(ElseClause());
            return ifStatement;
        }
        Node ElseClause()
        {
            //ElseClause ➔ Else_If_Statment | Else_Statment | end
            Node elseClause = new Node("ElseClause");
            if (CheckForNull(Token_Class.Elseif))
                elseClause.Children.Add(Else_If_Statment());
            else if (CheckForNull(Token_Class.Else))
                elseClause.Children.Add(Else_Statment());
            else
                elseClause.Children.Add(Match(Token_Class.End));
            return elseClause;
        }

        Node Else_If_Statment()
        {
            //Else_If_Statment ➔ elseif Condition_Statement then Statements ElseClause
            Node elseIfStatment = new Node("Else_If_Statment");
            elseIfStatment.Children.Add(Match(Token_Class.Elseif));
            elseIfStatment.Children.Add(Condition_Statement());
            elseIfStatment.Children.Add(Match(Token_Class.Then));
            elseIfStatment.Children.Add(Statements());
            elseIfStatment.Children.Add(ElseClause());
            return elseIfStatment;
        }

        Node Else_Statment()
        {
            //Else_Statment ➔ else Statements end
            Node elseStatment = new Node("Else_Statment");
            elseStatment.Children.Add(Match(Token_Class.Else));
            elseStatment.Children.Add(Statements());
            elseStatment.Children.Add(Match(Token_Class.End));
            return elseStatment;
        }

        public bool CheckForNull(Token_Class token)
        {
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == token)
                return true;
            return false;
        }

        public Node Match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count && ExpectedToken == TokenStream[InputPointer].token_type)
            {
                InputPointer++;
                Node newNode = new Node(ExpectedToken.ToString());

                return newNode;

            }

            else
            {
                if (InputPointer < TokenStream.Count)
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        " found\r\n"
                        + " at " + InputPointer.ToString() + "\n");

                    InputPointer++;
                }
                else
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and nothing was found\r\n");

                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}