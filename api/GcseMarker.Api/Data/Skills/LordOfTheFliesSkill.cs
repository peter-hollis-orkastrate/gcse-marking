namespace GcseMarker.Api.Data.Skills;

public static class LordOfTheFliesSkill
{
    public static SkillDefinition Definition { get; } = new()
    {
        Id = "lord-of-the-flies",
        Name = "AQA - English Literature - Lord of the Flies",
        Description = "GCSE English Literature essay marker for Lord of the Flies, aligned with Grade 9-1 marking criteria. Use when asked to mark, assess, grade, or provide feedback on a Lord of the Flies essay, or when a user uploads an essay about Lord of the Flies for evaluation. Provides band-level assessment (4-5, 6-7, 8-9), detailed feedback against official criteria, and specific improvement suggestions.",
        Subject = "English Literature",

        SystemPrompt = """
# Lord of the Flies Essay Marker (GCSE 9-1)

Mark Lord of the Flies essays against GCSE English Literature criteria, providing constructive feedback aligned with exam board standards.

## Marking Process

1. Read the essay and the question being answered
2. Assess against each criterion in the mark scheme (see references/mark-scheme.md)
3. Identify the best-fit grade band
4. Provide specific feedback with examples from the student's work
5. Give 2-3 actionable improvement targets

## Output Format

Structure feedback as follows:

### Overall Grade Band: [4-5 / 6-7 / 8-9]

### Criterion-by-Criterion Assessment

**Personal Response to Text**
- What the student did well
- What could be improved
- Current level for this criterion

**Analysis of Language, Form & Structure**
- What the student did well
- What could be improved
- Current level for this criterion

**Use of Evidence/Quotations**
- What the student did well
- What could be improved
- Current level for this criterion

**Context**
- What the student did well
- What could be improved
- Current level for this criterion

**SPaG (Spelling, Punctuation, Grammar)**
- What the student did well
- What could be improved
- Current level for this criterion

### Strengths (2-3 specific examples from the essay)

### Targets for Improvement (2-3 specific, actionable points)

### What Would Push This to the Next Band?

## Key Principles

- Be encouraging but honest
- Always give specific examples from the student's work
- Reference what examiners are looking for
- Make improvement targets actionable and specific
- Remember this is formative feedback to help the student improve
""",

        MarkScheme = """
# GCSE English Literature Mark Scheme - Lord of the Flies

## Grade Band Descriptors

### Grade 8-9 (Top Band)

An answer at this level:

- Shows an **insightful and critical** personal response to the text
- **Closely and perceptively analyses** how the writer uses language, form and structure to create meaning and affect the reader, making use of **highly relevant subject terminology**
- Supports arguments with **well-integrated, highly relevant and precise examples** from the text
- Gives a **detailed exploration** of the relationship between the text and its context
- Uses **highly varied vocabulary and sentence types**, with mostly accurate spelling and punctuation

**Key indicators:** Original interpretations, perceptive analysis, seamless quote integration, sophisticated contextual links, fluid academic writing.

---

### Grade 6-7 (Upper Middle Band)

An answer at this level:

- Shows a **critical and observant** personal response to the text
- Includes a **thorough exploration** of how the writer uses language, form and structure to create meaning and affect the reader, making use of **appropriate subject terminology**
- Supports arguments with **integrated, well-chosen examples** from the text
- **Explores** the relationship between the text and its context
- Uses a **substantial range of vocabulary and sentence types**, with generally accurate spelling and punctuation

**Key indicators:** Clear analysis with terminology, good quote selection and embedding, relevant context that connects to points, confident writing style.

---

### Grade 4-5 (Middle Band)

An answer at this level:

- Shows a **thoughtful and clear** personal response to the text
- **Examines** how the writer uses language, form and structure to create meaning and affect the reader, making **some use of relevant subject terminology**
- **Integrates appropriate examples** from the text
- Shows an **understanding of contextual factors**
- Uses a **moderate range of vocabulary and sentence types**, without spelling and punctuation errors which make the meaning unclear

**Key indicators:** Sound understanding, some analysis with terminology, quotes used but may not always be embedded, context mentioned but may not always connect to analysis, clear but straightforward expression.

---

## Assessment Objectives Breakdown

### AO1: Response to Text
- Understanding of the text
- Personal response and engagement
- Quality of argument/thesis

### AO2: Analysis of Writer's Methods
- Language analysis (word choice, imagery, figurative language)
- Form analysis (genre, structure of the whole novel)
- Structure analysis (how the text is organised, narrative techniques)
- Use of subject terminology
- Effect on reader

### AO3: Context
- Historical context (post-WWII, Cold War, nuclear anxiety)
- Social context (British class system, public school culture, colonialism)
- Biographical context (Golding's wartime experience as naval officer)
- Literary context (allegory, dystopian fiction, the island narrative tradition)
- Philosophical context (original sin vs civilisation, Hobbes vs Rousseau)
- How context shapes meaning
- How readers then vs now might respond

### AO4: SPaG (Spelling, Punctuation, Grammar)
- Spelling accuracy
- Punctuation accuracy
- Grammar and sentence construction
- Vocabulary range
- Overall clarity of expression

---

## What Distinguishes Each Band

| Aspect | Grade 4-5 | Grade 6-7 | Grade 8-9 |
|--------|-----------|-----------|-----------|
| Response | Thoughtful, clear | Critical, observant | Insightful, critical |
| Analysis | Examines | Thorough exploration | Close, perceptive analysis |
| Terminology | Some use | Appropriate use | Highly relevant use |
| Evidence | Appropriate examples | Well-chosen, integrated | Precise, well-integrated |
| Context | Understanding shown | Explores relationship | Detailed exploration |
| Expression | Moderate range, clear | Substantial range, accurate | Highly varied, accurate |

---

## Common Features of Top-Band Answers

1. **Original interpretation** - not just recycling standard views
2. **Perceptive word-level analysis** - why *this* specific word?
3. **Embedded quotations** - woven into sentences, not bolted on
4. **Sophisticated linking** - between points, to context, across the text
5. **Awareness of Golding's craft** - why he made these choices
6. **Consideration of reader response** - both 1950s and modern
7. **Confident academic voice** - precise, varied, fluent

## Common Weaknesses to Watch For

1. **Retelling the plot** instead of analysing
2. **Feature-spotting** - identifying techniques without explaining effect
3. **Bolted-on quotes** - not embedded in sentences
4. **Context as add-on** - mentioned but not linked to analysis
5. **Repetitive phrasing** - "This shows..." repeatedly
6. **Vague terminology** - using terms incorrectly or imprecisely
7. **Assertions without evidence** - claims not backed by quotes
8. **Over-simplifying the allegory** - treating symbols as one-dimensional
""",

        EssayTechniques = """
# Essay Techniques for Lord of the Flies - What Good Practice Looks Like

## The P.E.E.D. Structure

Each paragraph should follow Point, Example, Explain, Develop:

**Point** - Make a clear argument that answers the question
**Example** - Support with a quote or specific reference
**Explain** - Analyse what the quote shows and how it achieves this
**Develop** - Extend the analysis (context, alternative interpretation, link to wider themes)

### Example of P.E.E.D. in action:

> Golding presents Jack as increasingly savage throughout the novel. When Jack first attempts to kill a pig, he hesitates because of "the enormity of the knife descending and cutting into living flesh." The word "enormity" suggests that Jack still recognises the moral weight of killing, and the focus on "living flesh" shows his awareness of the pig as a sentient creature. However, this moral hesitation disappears entirely as the novel progresses, reflecting Golding's belief that civilised behaviour is only a thin veneer.

---

## Embedding Quotations

### Poor Practice (bolted-on quotes):
> Simon is different from the other boys. "Simon found for them the fruit they could not reach."

### Good Practice (embedded quotes):
> Simon's role as a Christ-like figure is established early when he selflessly "found for them the fruit they could not reach," positioning him as a provider and nurturer in contrast to the hunters.

### Tips for embedding:
- Quotes should flow naturally within your sentence
- Use short phrases rather than long passages
- The quote should be grammatically part of your sentence
- Always explain the significance of the quote

---

## Writing About Language

### What to look for:
- **Word choice** - why this specific word?
- **Imagery** - similes, metaphors, personification
- **Repeated words/phrases** - why repeated?
- **Contrasts** - opposites placed together
- **Sounds** - alliteration, sibilance, harsh/soft sounds
- **Symbolic language** - objects representing ideas

### Example analysis:
> Golding describes the dead parachutist as a "sign came down from the world of grown-ups." The word "sign" has religious connotations, suggesting prophecy or divine message, yet this "sign" is a corpse from war. This bitter irony undermines any hope that adults represent salvation, as the adult world is shown to be just as violent and chaotic as the island.

---

## Writing About Structure

### What to look for:
- **Order of events** - why this sequence?
- **Foreshadowing** - hints about what's to come
- **Parallels** - similar events/situations
- **Contrasts** - juxtaposed scenes
- **Beginnings and endings** - of chapters, the whole novel
- **Turning points** - moments of change
- **The frame narrative** - how the novel opens and closes

### Example analysis:
> Golding structures the novel so that Simon's murder immediately follows his discovery of the truth about the beast. This juxtaposition is devastating: at the moment when salvation is possible, the boys destroy it. Structurally, this marks the point of no return, after which rescue by the naval officer feels hollow rather than triumphant.

---

## Writing About Form

### What to consider for Lord of the Flies:
- **Allegory** - characters and events representing broader ideas
- **Third-person narration** - omniscient perspective, access to thoughts
- **Fable/parable elements** - moral lessons embedded in narrative
- **The island setting** - microcosm of society
- **Dialogue vs description** - what Golding chooses to show vs tell

### Example analysis:
> Golding's use of allegory means that characters function on multiple levels. Piggy represents rationality and democratic values, but he is also a fully realised character with insecurities about his weight and asthma. This dual function allows Golding to explore abstract ideas about civilisation while maintaining emotional engagement with the characters' fates.

---

## Including Context Effectively

### Poor Practice (context as add-on):
> Lord of the Flies was written after World War II. The boys become violent on the island.

### Good Practice (context integrated with analysis):
> The boys' descent into tribal violence reflects Golding's experiences as a naval officer during World War II, where he witnessed ordinary men commit atrocities. His portrayal of Jack's hunters as a "tribe" with painted faces deliberately evokes the dehumanising propaganda used during the war, challenging readers to recognise savagery in themselves rather than projecting it onto foreign "others."

### Key contextual areas for Lord of the Flies:
- **World War II** - Golding's naval service, witnessing human capacity for evil
- **The Cold War** - nuclear anxiety, the atomic bomb (referenced in the novel)
- **British Empire decline** - post-colonial questioning of "civilised" values
- **Public school system** - the boys' backgrounds, class and leadership
- **Original sin vs Rousseau** - are humans inherently good or evil?
- **The Coral Island** - Golding's subversion of Victorian boys' adventure stories
- **1950s Britain** - post-war optimism vs underlying anxieties

---

## Introduction Technique

### What a good introduction does:
- Directly addresses the question
- Sets up the argument/thesis
- Shows understanding of the text
- Doesn't waste words on plot summary

### Example (for "How does Golding present the theme of civilisation vs savagery?"):

**Weaker:**
> Lord of the Flies is about a group of boys who crash on an island and become savage. Golding shows that civilisation is important because without it people become violent.

**Stronger:**
> Golding presents civilisation not as humanity's natural state but as a fragile construct that requires constant maintenance. Through the progressive breakdown of democratic structures on the island, symbolised by the deteriorating conch, Golding suggests that savagery is not learned but merely suppressed by social conventions—and that this suppression can fail terrifyingly quickly.

---

## Conclusion Technique

### What a good conclusion does:
- Summarises the argument (briefly)
- Answers the question directly
- May offer a final insight or alternative view
- Does NOT introduce new points or evidence

### What to avoid:
- Starting with "In conclusion..."
- Simply repeating what you've already said
- Introducing new arguments or quotations
- Ending weakly or trailing off

---

## Linking Paragraphs

Use linking words and phrases to show how ideas connect:

- **Building on a point:** Furthermore, Moreover, Additionally
- **Contrasting:** However, Conversely, On the other hand
- **Showing consequence:** Therefore, Consequently, As a result
- **Comparing:** Similarly, Likewise, In the same way
- **Emphasising:** Indeed, Significantly, Crucially

### Example of linked paragraphs:
> [End of paragraph about Ralph's leadership]...This demonstrates how Ralph's authority depends entirely on the boys' willingness to follow democratic rules.
>
> Conversely, Jack's power derives from fear and the promise of hunting, which proves far more compelling once the constraints of civilisation begin to erode...

---

## Subject Terminology to Use

### Language terms:
Metaphor, simile, personification, imagery, symbolism, connotations, alliteration, sibilance, pathetic fallacy, juxtaposition, irony, biblical allusion

### Structure terms:
Foreshadowing, dramatic irony, climax, turning point, parallel, contrast, cyclical structure, frame narrative, episodic structure

### Form/narrative terms:
Allegory, parable, fable, third-person omniscient narrator, microcosm, dystopia, narrative perspective, free indirect discourse

### Character/theme terms:
Protagonist, antagonist, foil character, characterisation, motif, theme, symbol, archetype, Christ figure, anti-hero

## Key Symbols to Analyse

- **The conch** - democracy, order, civilised discourse
- **Piggy's glasses** - rationality, intelligence, scientific progress
- **The fire** - hope, rescue, connection to civilisation (but also destruction)
- **The beast** - primal fear, the evil within humanity
- **The Lord of the Flies** - the devil, humanity's inherent evil
- **Face paint** - loss of identity, liberation from moral constraints
- **The island itself** - Eden, microcosm of society, testing ground
"""
    };
}
