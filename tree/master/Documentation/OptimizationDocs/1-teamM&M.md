---


---

<h1 id="передаточная-функция-в-задаче-о-распространении-констант">Передаточная функция в задаче о распространении констант</h1>
<h2 id="постановка-задачи">Постановка задачи</h2>
<p>Реализовать передаточную функцию для задачи распространения констант.</p>
<h2 id="команда-—-исполнитель">Команда — исполнитель</h2>
<p>M&amp;M</p>
<h2 id="зависимости">Зависимости</h2>
<p>Зависит от</p>
<ul>
<li>Базовых блоков</li>
<li>Трехадресного кода</li>
<li>Графа потока управления</li>
<li>Оператора сбора /\ и отображение m в задаче о распространении констант</li>
</ul>
<h2 id="теория">Теория</h2>
<p>Каждая переменная в некоторой таблице имеет одно из значений в полурешетке - <em>UNDEF</em> (undefigned), <em>const</em>, <em>NAC</em> (not a const). Таблица является декартовым произведением полурешеток, и следовательно, сама полурешетка. Таким образом, элементом данных будет отображение <em>m</em> на соответствующее значение полурешетки.</p>
<ol>
<li>
<p>Если s не является присваиванием, то f тождественна, т.е. m = m’</p>
</li>
<li>
<p>Если s присваивание, то для каждого v != x: m’(v) = m(v)</p>
</li>
<li>
<p>Если s присваивание константе, то m’(x) = const</p>
</li>
<li>
<p>Если s: x=y+z, то</p>
<ul>
<li>m’(x) = m(y) + m(z), если m(y) и m(z) - const</li>
<li>m’(x) = NAC, если m(y) или m(z) - NAC</li>
<li>m’(x) = UNDEF в остальных случаях</li>
</ul>
</li>
</ol>
<h2 id="реализация">Реализация</h2>
<p>Для решения поставленной задачи был реализован следующий метод :</p>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token punctuation">.</span><span class="token punctuation">.</span><span class="token punctuation">.</span>
<span class="token comment">// Использует итерационный алгоритм для задачи распространения констант OUT[B] = fB(IN[B])</span>
<span class="token comment">// На вход получает _in Stream из которого создается DataStreamValue - Поток данных(реализация на множествах для ITA)</span>
<span class="token comment">// Выход новый Stream после изменения значений для присваиваний из tACodeLines</span>
<span class="token comment">// где SemilatticeStreamValue - обёртка над именем переменной и её значением из полурешётки констант SemilatticeValue</span>
<span class="token comment">// где DataStreamTable - Поток данных(реализация на словаре для реализации алгоритма и более удобной работы)</span>
<span class="token keyword">public</span> HashSet<span class="token operator">&lt;</span>SemilatticeStreamValue<span class="token operator">&gt;</span> <span class="token function">Calculate</span><span class="token punctuation">(</span>HashSet<span class="token operator">&lt;</span>SemilatticeStreamValue<span class="token operator">&gt;</span> _in<span class="token punctuation">,</span> ThreeAddressCode tACodeLines<span class="token punctuation">)</span>
<span class="token punctuation">{</span>
	basicBlock <span class="token operator">=</span> tACodeLines<span class="token punctuation">;</span>
	<span class="token keyword">var</span> result <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token class-name">HashSet</span><span class="token operator">&lt;</span>SemilatticeStreamValue<span class="token operator">&gt;</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
	<span class="token keyword">var</span> dataStreamValue1 <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token class-name">DataStreamValue</span><span class="token punctuation">(</span>_in<span class="token punctuation">)</span><span class="token punctuation">;</span>
	<span class="token keyword">var</span> dataStreamValue2 <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token class-name">DataStreamValue</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
	<span class="token keyword">foreach</span> <span class="token punctuation">(</span>TacNode node <span class="token keyword">in</span> tACodeLines<span class="token punctuation">)</span>
	<span class="token punctuation">{</span>
		<span class="token keyword">if</span> <span class="token punctuation">(</span>node <span class="token keyword">is</span> TacAssignmentNode assign<span class="token punctuation">)</span>
		<span class="token punctuation">{</span>
			<span class="token keyword">var</span> idVar <span class="token operator">=</span> assign<span class="token punctuation">.</span>LeftPartIdentifier<span class="token punctuation">;</span>
			<span class="token keyword">var</span> value1 <span class="token operator">=</span> assign<span class="token punctuation">.</span>FirstOperand<span class="token punctuation">;</span>
			<span class="token keyword">var</span> operation <span class="token operator">=</span> assign<span class="token punctuation">.</span>Operation<span class="token punctuation">;</span>
			<span class="token keyword">var</span> value2 <span class="token operator">=</span> assign<span class="token punctuation">.</span>SecondOperand<span class="token punctuation">;</span>
			<span class="token keyword">var</span> table <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token class-name">DataStreamTable</span><span class="token punctuation">(</span>dataStreamValue1<span class="token punctuation">.</span>Stream<span class="token punctuation">,</span> dataStreamValue2<span class="token punctuation">.</span>Stream<span class="token punctuation">)</span><span class="token punctuation">;</span>
			<span class="token keyword">if</span> <span class="token punctuation">(</span><span class="token function">IsSimpleAssignNode</span><span class="token punctuation">(</span>assign<span class="token punctuation">)</span><span class="token punctuation">)</span>
				dataStreamValue2<span class="token punctuation">.</span><span class="token function">ChangeStreamValue</span><span class="token punctuation">(</span>idVar<span class="token punctuation">,</span> table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span>value1<span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
			<span class="token keyword">else</span>
			<span class="token punctuation">{</span>
				<span class="token keyword">var</span> semVal1 <span class="token operator">=</span> table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span>value1<span class="token punctuation">)</span><span class="token punctuation">;</span>
				<span class="token keyword">var</span> semVal2 <span class="token operator">=</span> table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span>value2<span class="token punctuation">)</span><span class="token punctuation">;</span>
				<span class="token keyword">if</span> <span class="token punctuation">(</span>semVal1<span class="token punctuation">.</span>TypeValue <span class="token operator">==</span> SemilatticeValueEnum<span class="token punctuation">.</span>CONST <span class="token operator">&amp;&amp;</span> semVal2<span class="token punctuation">.</span>TypeValue <span class="token operator">==</span> SemilatticeValueEnum<span class="token punctuation">.</span>CONST<span class="token punctuation">)</span>
				<span class="token punctuation">{</span>
					<span class="token keyword">double</span> val1 <span class="token operator">=</span> <span class="token keyword">double</span><span class="token punctuation">.</span><span class="token function">Parse</span><span class="token punctuation">(</span>semVal1<span class="token punctuation">.</span>ConstValue<span class="token punctuation">)</span><span class="token punctuation">;</span>
					<span class="token keyword">double</span> val2 <span class="token operator">=</span> <span class="token keyword">double</span><span class="token punctuation">.</span><span class="token function">Parse</span><span class="token punctuation">(</span>semVal2<span class="token punctuation">.</span>ConstValue<span class="token punctuation">)</span><span class="token punctuation">;</span>
					<span class="token keyword">double</span> val3 <span class="token operator">=</span> <span class="token number">0</span><span class="token punctuation">;</span>
					<span class="token keyword">switch</span> <span class="token punctuation">(</span>operation<span class="token punctuation">)</span>
					<span class="token punctuation">{</span>
						<span class="token keyword">case</span> <span class="token string">"+"</span><span class="token punctuation">:</span>
						val3 <span class="token operator">=</span> val1 <span class="token operator">+</span> val2<span class="token punctuation">;</span>
						<span class="token keyword">break</span><span class="token punctuation">;</span>
						<span class="token keyword">case</span> <span class="token string">"-"</span><span class="token punctuation">:</span>
						val3 <span class="token operator">=</span> val1 <span class="token operator">-</span> val2<span class="token punctuation">;</span>
						<span class="token keyword">break</span><span class="token punctuation">;</span>
						<span class="token keyword">case</span> <span class="token string">"/"</span><span class="token punctuation">:</span>
						val3 <span class="token operator">=</span> val1 <span class="token operator">/</span> val2<span class="token punctuation">;</span>
						<span class="token keyword">break</span><span class="token punctuation">;</span>
						<span class="token keyword">case</span> <span class="token string">"*"</span><span class="token punctuation">:</span>
						val3 <span class="token operator">=</span> val1 <span class="token operator">*</span> val2<span class="token punctuation">;</span>
						<span class="token keyword">break</span><span class="token punctuation">;</span>
					<span class="token punctuation">}</span>
					dataStreamValue2<span class="token punctuation">.</span><span class="token function">ChangeStreamValue</span><span class="token punctuation">(</span>idVar<span class="token punctuation">,</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>CONST<span class="token punctuation">,</span> val3<span class="token punctuation">.</span><span class="token function">ToString</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
				<span class="token punctuation">}</span>
				<span class="token keyword">else</span> <span class="token keyword">if</span> <span class="token punctuation">(</span>semVal1<span class="token punctuation">.</span>TypeValue <span class="token operator">==</span> SemilatticeValueEnum<span class="token punctuation">.</span>NAC <span class="token operator">||</span> semVal2<span class="token punctuation">.</span>TypeValue <span class="token operator">==</span> SemilatticeValueEnum<span class="token punctuation">.</span>NAC<span class="token punctuation">)</span>
				<span class="token punctuation">{</span>
					dataStreamValue2<span class="token punctuation">.</span><span class="token function">ChangeStreamValue</span><span class="token punctuation">(</span>idVar<span class="token punctuation">,</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>NAC<span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
				<span class="token punctuation">}</span>
				<span class="token keyword">else</span>
				<span class="token punctuation">{</span>
					dataStreamValue2<span class="token punctuation">.</span><span class="token function">ChangeStreamValue</span><span class="token punctuation">(</span>idVar<span class="token punctuation">,</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>UNDEF<span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
				<span class="token punctuation">}</span>
			<span class="token punctuation">}</span>
		<span class="token punctuation">}</span>
	<span class="token punctuation">}</span>
	<span class="token keyword">var</span> dataStreamValue3 <span class="token operator">=</span> dataStreamValue1 <span class="token operator">^</span> dataStreamValue2<span class="token punctuation">;</span>
	<span class="token keyword">return</span> dataStreamValue3<span class="token punctuation">.</span>Stream<span class="token punctuation">;</span>
<span class="token punctuation">}</span>
<span class="token punctuation">.</span><span class="token punctuation">.</span><span class="token punctuation">.</span>
</code></pre>
<h2 id="тесты">Тесты</h2>
<pre class=" language-csharp"><code class="prism  language-csharp"><span class="token keyword">var</span> constDistribFun <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token class-name">ConstDistribFunction</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
<span class="token keyword">var</span> emptySet <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token class-name">HashSet</span><span class="token operator">&lt;</span>SemilatticeStreamValue<span class="token operator">&gt;</span><span class="token punctuation">(</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token keyword">var</span> f1 <span class="token operator">=</span> constDistribFun<span class="token punctuation">.</span><span class="token function">Calculate</span><span class="token punctuation">(</span>emptySet<span class="token punctuation">,</span> <span class="token function">GetCodeLinesByText</span><span class="token punctuation">(</span><span class="token string">"x=2;y=3;"</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
<span class="token keyword">var</span> f1Table <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token class-name">DataStreamTable</span><span class="token punctuation">(</span>f1<span class="token punctuation">)</span><span class="token punctuation">;</span>
Debug<span class="token punctuation">.</span><span class="token function">Assert</span><span class="token punctuation">(</span>f1Table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span><span class="token string">"x"</span><span class="token punctuation">)</span> <span class="token operator">==</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>CONST<span class="token punctuation">,</span> <span class="token string">"2"</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
Debug<span class="token punctuation">.</span><span class="token function">Assert</span><span class="token punctuation">(</span>f1Table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span><span class="token string">"y"</span><span class="token punctuation">)</span> <span class="token operator">==</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>CONST<span class="token punctuation">,</span> <span class="token string">"3"</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
Debug<span class="token punctuation">.</span><span class="token function">Assert</span><span class="token punctuation">(</span>f1Table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span><span class="token string">"z"</span><span class="token punctuation">)</span> <span class="token operator">==</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>UNDEF<span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>

<span class="token keyword">var</span> f2 <span class="token operator">=</span> constDistribFun<span class="token punctuation">.</span><span class="token function">Calculate</span><span class="token punctuation">(</span>emptySet<span class="token punctuation">,</span> <span class="token function">GetCodeLinesByText</span><span class="token punctuation">(</span><span class="token string">"x=3;y=2;"</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
<span class="token keyword">var</span> f2Table <span class="token operator">=</span> <span class="token keyword">new</span> <span class="token class-name">DataStreamTable</span><span class="token punctuation">(</span>f2<span class="token punctuation">)</span><span class="token punctuation">;</span>
Debug<span class="token punctuation">.</span><span class="token function">Assert</span><span class="token punctuation">(</span>f2Table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span><span class="token string">"x"</span><span class="token punctuation">)</span> <span class="token operator">==</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>CONST<span class="token punctuation">,</span> <span class="token string">"3"</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
Debug<span class="token punctuation">.</span><span class="token function">Assert</span><span class="token punctuation">(</span>f2Table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span><span class="token string">"y"</span><span class="token punctuation">)</span> <span class="token operator">==</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>CONST<span class="token punctuation">,</span> <span class="token string">"2"</span><span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
Debug<span class="token punctuation">.</span><span class="token function">Assert</span><span class="token punctuation">(</span>f2Table<span class="token punctuation">.</span><span class="token function">GetValue</span><span class="token punctuation">(</span><span class="token string">"z"</span><span class="token punctuation">)</span> <span class="token operator">==</span> <span class="token keyword">new</span> <span class="token class-name">SemilatticeValue</span><span class="token punctuation">(</span>SemilatticeValueEnum<span class="token punctuation">.</span>UNDEF<span class="token punctuation">)</span><span class="token punctuation">)</span><span class="token punctuation">;</span>
</code></pre>
<h2 id="вывод">Вывод</h2>
<p>Используя методы, описанные выше, мы получили передаточную функцию в задачи распространения констант.</p>

