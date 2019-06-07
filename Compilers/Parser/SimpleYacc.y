%{
// Ёти объ€влени€ добавл€ютс€ в класс GPPGParser, представл€ющий собой парсер, генерируемый системой gppg
    public BlockNode root; //  орневой узел синтаксического дерева 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
	private bool InDefSect = false;
%}

%output = SimpleYacc.cs

%union { 
			public double dVal; 
			public int iVal; 
			public string sVal; 
			public Node nVal;
			public ExprNode eVal;
			public StatementNode stVal;
			public BlockNode blVal;
       }

%using System.IO;
%using ProgramTree;

%namespace SimpleParser

%start progr

%token BEGIN END CYCLE ASSIGN ASSIGNPLUS ASSIGNMINUS ASSIGNMULT SEMICOLON WRITE
PLUS MINUS MULT DIV OPEN_BRACKET CLOSE_BRACKET
OPEN_BLOCK CLOSE_BLOCK OPEN_SQUARE CLOSE_SQUARE
TRUE FALSE NO AND OR MORE LESS EQUAL NOT_EQUAL MORE_EQUAL LESS_EQUAL MOD
INT DOUBLE BOOL NOT
WHILE FOR TO PRINTLN IF ELSE COMMA LABEL COLON GOTO

%token <iVal> INUM 
%token <dVal> RNUM 
%token <sVal> ID

%type <eVal> expr ident T F S
%type <stVal> statement assign block  empty while for if println idenlist label goto
%type <blVal> stlist block

%%

progr   : stlist { root = $1; }
		;

stlist	: statement 
			{ 
				$$ = new BlockNode($1); 
			}
		| stlist statement 
			{ 
				$1.Add($2); 
				$$ = $1; 
			}
		;

statement: assign SEMICOLON { $$ = $1; }
		| block   { $$ = $1; }		
		| empty SEMICOLON  { $$ = $1; }
		| while   { $$ = $1; }
		| for { $$ = $1; }
		| println { $$ = $1; }
		| if { $$ = $1; }
		| label { $$ = $1; }
		| goto { $$ = $1; }
		| idenlist SEMICOLON { $$ = $1; }
		;

idenlist: TYPE ident 
		| idenlist COMMA ident 
		;

TYPE : INT | DOUBLE | BOOL;

empty	: { $$ = new EmptyNode(); }
		;
	
ident 	: ID 
		{
			// if (!InDefSect)
			//	if (!SymbolTable.vars.ContainsKey($1))
			//		throw new Exception("("+@1.StartLine+","+@1.StartColumn+"): ѕеременна€ "+$1+" не описана");
			$$ = new IdNode($1); 
		}
		| ID OPEN_SQUARE expr CLOSE_SQUARE
	;
	
assign 	: ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3); }
		| TYPE ident ASSIGN expr { $$ = new AssignNode($2 as IdNode, $4); }
		;

block	: OPEN_BLOCK stlist CLOSE_BLOCK { $$ = $2; }
		;

while	: WHILE OPEN_BRACKET expr  CLOSE_BRACKET statement { $$ = new WhileNode($3, $5); }
		;

for		: FOR OPEN_BRACKET assign TO expr CLOSE_BRACKET statement { $$ = new ForNode($3, $5, $7); }
		;

println	: PRINTLN OPEN_BRACKET expr CLOSE_BRACKET SEMICOLON
		;

if      : IF OPEN_BRACKET expr CLOSE_BRACKET statement { $$ = new IfNode($3, $5); }
        | IF OPEN_BRACKET expr CLOSE_BRACKET statement ELSE statement { $$ = new IfNode($3, $5, $7); }
		;

label	: LABEL INUM COLON { $$ = new LabelNode($2); }
		;

goto	: GOTO label { $$ = new GotoNode($2 as LabelNode); }
		;

expr    : T { $$ = $1; }
        | expr EQUAL T { $$ = new BinOpNode($1, $3, "=="); }
        | expr MORE T { $$ = new BinOpNode($1, $3, ">"); }
		| expr LESS T { $$ = new BinOpNode($1, $3, "<"); }
		| expr NOT_EQUAL T { $$ = new BinOpNode($1, $3, "!="); }
        | expr MORE_EQUAL T { $$ = new BinOpNode($1, $3, ">="); }
		| expr LESS_EQUAL T { $$ = new BinOpNode($1, $3, "<="); }
        ;
        
T       : F { $$ = $1 as ExprNode; }
        | T PLUS F { $$ = new BinOpNode ( $1, $3, "+"); }
        | T MINUS F { $$ = new BinOpNode ($1, $3, "-"); }
		| T OR F { $$ = new BinOpNode($1, $3, "||"); }
        ;

F       : S { $$ = $1 as ExprNode; }
        | F MULT S { $$ = new BinOpNode ( $1, $3, "*"); }
        | F DIV S { $$ = new BinOpNode ($1, $3, "/"); }
		| F MOD S { $$ = new BinOpNode($1, $3, "%"); }
		| F AND S { $$ = new BinOpNode($1, $3, "&&"); }
        ;
        
S       : ident { $$ = $1 as IdNode; }
		| ident OPEN_BRACKET CLOSE_BRACKET { $$ = new FunctionNode($1 as IdNode); }
        | NOT S { $$ = new LogicNotNode($2); }
		| MINUS S { $$ = new UnOpNode($2); }
        | INUM { $$ = new IntNumNode($1); }
        | RNUM { $$ = new DoubleNumNode($1); }
		| TRUE { $$ = new BoolNode(true); }
		| FALSE { $$ = new BoolNode(false); }
        | OPEN_BRACKET expr CLOSE_BRACKET { $$ = $2 as ExprNode; }
        ;
%%

