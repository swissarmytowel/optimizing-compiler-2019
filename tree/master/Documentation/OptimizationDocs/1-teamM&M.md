---


---

<h1 id="название-задачи">Название задачи</h1>
<p>Парсер языка и построение AST-дерева</p>
<h2 id="постановка-задачи">Постановка задачи</h2>
<p>Написать парсер языка на языке C# с использованием GPLex и Yacc. Реализовать построение синтаксического дерева программы.</p>
<h2 id="команда-—-исполнитель">Команда — исполнитель</h2>
<p>M&amp;M</p>
<h2 id="зависимости">Зависимости</h2>
<p>–</p>
<h2 id="теория">Теория</h2>
<p>Для решения данной задачи необходимо реализовать две составляющие: лексер и парсер языка.  <strong>Опр.</strong> Лексический анализатор (лексер) — это программа или часть программы, выполняющая лексический анализ. Лексер предназначен для разбиения входного потока символов на лексемы - отдельные, осмысленные единицы программы.</p>
<p>Основные задачи, которые выполняет лексер:</p>
<ul>
<li>Выделение идентификаторов и целых чисел</li>
<li>Выделение ключевых слов</li>
<li>Выделение символьных токенов</li>
</ul>
<p><strong>Опр.</strong>  Парсер (или синтаксический анализатор) — часть программы, преобразующей входные данные (как правило, текст) в структурированный формат. Парсер выполняет синтаксический анализ текста. Парсер принимает на вход поток лексем и формирует абстрактное синтаксическое дерево (AST).</p>
<h2 id="реализация">Реализация</h2>
<p>Для автоматического создания парсера создаются файлы SimpleLex.lex (описание лексического анализатора) и SimpleYacc.y (описание синтаксического анализатора). Код лексического и синтаксического анализаторов создаются на C# запуском командного файла generateParserScanner.bat.</p>
<p>Синтаксически управляемая трансляция состоит в том, что при разборе текста программы на каждое распознанное правило грамматики выполняется некоторое действие. Данные действия придают смысл трансляции (переводу) и поэтому мы называем их семантическими. Семантические действия записываются в .y-файле после правил в фигурных скобках и представляют собой код программы на C# (целевом языке компилятора).</p>
<p>Как правило, при трансляции программа переводится в другую форму, более приспособленную для анализа, дальнейших преобразований и генерации кода.</p>
<p>Мы будем переводить текст программы в так называемое синтаксическое дерево. Если синтаксическое дерево построено, то программа синтаксически правильная, и ее можно подвергать дальнейшей обработке.</p>
<p>В синтаксическое дерево включаются узлы, соответствующие всем синтаксическим конструкциям языка. Атрибутами этих узлов являются их существенные характеристики. Например, для узла оператора присваивания AssignNode такими атрибутами являются IdNode - идентификатор в левой части оператора присваивания и ExprNode - выражение в правой части оператора присваивания.</p>
<h4 id="парсер-языка">Парсер языка</h4>
<p>Парсер был реализован для языка со следующим синтаксисом:</p>
<pre class=" language-csharp"><code class="prism  language-csharp">a <span class="token operator">=</span> <span class="token number">777</span><span class="token punctuation">;</span> <span class="token comment">// оператор присваивания</span>
</code></pre>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token comment">// пример арифметических операций</span>
a <span class="token operator">=</span> a <span class="token operator">-</span> b<span class="token punctuation">;</span>
c <span class="token operator">=</span> a <span class="token operator">+</span> b<span class="token punctuation">;</span>
a <span class="token operator">=</span> a <span class="token operator">*</span> <span class="token number">3</span><span class="token punctuation">;</span>
a <span class="token operator">=</span> <span class="token number">5</span> <span class="token operator">*</span> b<span class="token punctuation">;</span>
</code></pre>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token comment">// пример операторов сравнения</span>
c <span class="token operator">=</span> a <span class="token operator">&lt;</span> b<span class="token punctuation">;</span>
c <span class="token operator">=</span> a <span class="token operator">&gt;</span> b<span class="token punctuation">;</span>
c <span class="token operator">=</span> a <span class="token operator">==</span> b<span class="token punctuation">;</span>
a <span class="token operator">=</span> <span class="token number">5</span> <span class="token operator">*</span> b<span class="token punctuation">;</span>
<span class="token comment">// логическое "нет"</span>
c <span class="token operator">=</span> <span class="token operator">!</span>a<span class="token punctuation">;</span>
</code></pre>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token comment">// полная форма условного оператора</span>
<span class="token keyword">if</span> <span class="token punctuation">(</span>a <span class="token operator">&lt;</span> b<span class="token punctuation">)</span>    
	a <span class="token operator">=</span> <span class="token number">555</span><span class="token punctuation">;</span>
<span class="token keyword">else</span>
<span class="token punctuation">{</span>
    b <span class="token operator">=</span> <span class="token number">666</span><span class="token punctuation">;</span>
    с <span class="token operator">=</span> <span class="token number">777</span><span class="token punctuation">;</span>
<span class="token punctuation">}</span>
<span class="token comment">// сокращенная форма условного оператора </span>
<span class="token keyword">if</span> <span class="token punctuation">(</span>b <span class="token operator">==</span> c<span class="token punctuation">)</span>    
	c <span class="token operator">=</span> <span class="token number">666</span><span class="token punctuation">;</span>
</code></pre>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token comment">// операторы циклов</span>
<span class="token comment">// цикл while</span>
<span class="token keyword">while</span><span class="token punctuation">(</span>a <span class="token operator">&lt;</span> b<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
    <span class="token punctuation">.</span><span class="token punctuation">.</span><span class="token punctuation">.</span>
<span class="token punctuation">}</span>
<span class="token comment">// цикл for</span>
<span class="token keyword">for</span><span class="token punctuation">(</span>i <span class="token operator">=</span> <span class="token number">0</span> to <span class="token number">10</span><span class="token punctuation">)</span>
<span class="token punctuation">{</span>    
	<span class="token punctuation">.</span><span class="token punctuation">.</span><span class="token punctuation">.</span>
<span class="token punctuation">}</span>
</code></pre>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token comment">// оператор вывода</span>
<span class="token function">print</span><span class="token punctuation">(</span>a<span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token comment">// оператор goto</span>
<span class="token keyword">goto</span> l <span class="token number">7</span><span class="token punctuation">:</span>
<span class="token comment">// переход по метке</span>
l <span class="token number">7</span><span class="token punctuation">:</span>
</code></pre>
<p>Для создания парсера использовались Yacc и GPLex, были созданы соответствующие файлы .y и .lex.<br>
Пример содержимого .y файла:</p>
<pre><code>%token &lt;iVal&gt; INUM
%token &lt;dVal&gt; RNUM
%token &lt;sVal&gt; ID

%type &lt;eVal&gt; expr ident T F S
%type &lt;stVal&gt; statement assign block
 empty while for if println
 idenlist label goto
%type &lt;blVal&gt; stlist block

%%
progr : stlist { root = $1; }
	  ;
statement: assign SEMICOLON { $$ = $1; }
		| block { $$ = $1; }
		| empty SEMICOLON  { $$ = $1; }
		| while { $$ = $1; }
		| for { $$ = $1; }
		| println { $$ = $1; }
		| if { $$ = $1; }
		| label { $$ = $1; }
		| goto { $$ = $1; }
		| idenlist SEMICOLON { $$ = $1;}
		;
</code></pre>
<p>Пример содержимого .lex файла:</p>
<pre><code>{REALNUM} {
	yylval.dVal = double.Parse(yytext);
	return (int)Tokens.RNUM;
}
{ID}  {
	int res = ScannerHelper.GetIDToken(yytext);
	if (res == (int)Tokens.ID)
		yylval.sVal = yytext;
	return res;
}
"=" { return (int)Tokens.ASSIGN; }
";" { return (int)Tokens.SEMICOLON; }
":" { return (int)Tokens.COLON; }
</code></pre>
<h4 id="построение-ast-дерева">Построение AST-дерева</h4>
<p>Для построения AST дерева были созданы классы для каждого типа узла:</p>
<ul>
<li>
<p>Node.cs - базовый класс для всех узлов</p>
</li>
<li>
<p>ExprNode.cs  - базовый класс для выражений</p>
</li>
<li>
<p>AssignNode.cs - операция присваивания</p>
</li>
<li>
<p>BinOpNode.cs  - класс для бинарных операций</p>
</li>
<li>
<p>UnOpNode - класс для унарных операций</p>
</li>
<li>
<p>IntNumNode.cs - класс для целочисленных констант</p>
</li>
<li>
<p>IdNode.cs - класс для идентификаторов</p>
</li>
<li>
<p>StatementNode.cs - базовый класс для всех операторов</p>
</li>
<li>
<p>BlockNode.cs  - класс для блока</p>
</li>
<li>
<p>WhileNode.cs  - класс для цикла  <em>while</em></p>
</li>
<li>
<p>ForNode.cs - класс для цикла  <em>for</em></p>
</li>
<li>
<p>GotoNode.cs - класс для  <em>goto</em></p>
</li>
<li>
<p>IfNode.cs - класс для оператора сравнения</p>
</li>
<li>
<p>LabelNode.cs - класс метки goto</p>
</li>
<li>
<p>PrintNode.cs - класс оператора вывода</p>
</li>
<li>
<p>EmptyNode.cs - класс для пустого узла</p>
</li>
</ul>
<p>Пример кода, описывающего оператор цикла  <em>while</em>:</p>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token keyword">public</span> <span class="token keyword">class</span> <span class="token class-name">WhileNode</span> <span class="token punctuation">:</span> StatementNode
<span class="token punctuation">{</span>
	<span class="token keyword">public</span> ExprNode Expr <span class="token punctuation">{</span> <span class="token keyword">get</span><span class="token punctuation">;</span> <span class="token keyword">set</span><span class="token punctuation">;</span> <span class="token punctuation">}</span>
	<span class="token keyword">public</span> StatementNode Stat <span class="token punctuation">{</span> <span class="token keyword">get</span><span class="token punctuation">;</span> <span class="token keyword">set</span><span class="token punctuation">;</span> <span class="token punctuation">}</span>
	<span class="token keyword">public</span> <span class="token function">WhileNode</span><span class="token punctuation">(</span>ExprNode expr<span class="token punctuation">,</span> StatementNode stat<span class="token punctuation">)</span>
	<span class="token punctuation">{</span>
		Expr <span class="token operator">=</span> expr<span class="token punctuation">;</span>
		Stat <span class="token operator">=</span> stat<span class="token punctuation">;</span>
	<span class="token punctuation">}</span>
	<span class="token keyword">public</span> <span class="token keyword">override</span> <span class="token keyword">void</span> <span class="token function">Visit</span><span class="token punctuation">(</span>Visitor v<span class="token punctuation">)</span>
	<span class="token punctuation">{</span>
		v<span class="token punctuation">.</span><span class="token function">VisitWhileNode</span><span class="token punctuation">(</span><span class="token keyword">this</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
	<span class="token punctuation">}</span>
	<span class="token keyword">public</span> <span class="token keyword">override</span> <span class="token keyword">string</span> <span class="token function">ToString</span><span class="token punctuation">(</span><span class="token punctuation">)</span>
	<span class="token punctuation">{</span>
		<span class="token keyword">return</span> <span class="token string">"while("</span> <span class="token operator">+</span> Expr <span class="token operator">+</span> <span class="token string">")\n"</span> <span class="token operator">+</span> Stat<span class="token punctuation">;</span>
	<span class="token punctuation">}</span>
<span class="token punctuation">}</span>
</code></pre>
<h2 id="тесты">Тесты</h2>
<p>Узнать как должны выглядить тесты в докуметации.</p>
<h2 id="вывод">Вывод</h2>
<p>Был реализован парсер языка и построено AST-дерево.</p>

