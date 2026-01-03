namespace GcseMarker.Api.Data.Skills;

public static class MacbethSkill
{
    public static SkillDefinition Definition { get; } = new()
    {
        Id = "macbeth",
        Name = "AQA - English Literature - Macbeth",
        Description = "GCSE English Literature essay marker for Macbeth, aligned with Grade 9-1 marking criteria. Use when asked to mark, assess, grade, or provide feedback on a Macbeth essay, or when a user uploads an essay about Macbeth for evaluation. Provides band-level assessment (4-5, 6-7, 8-9), detailed feedback against official criteria, and specific improvement suggestions.",
        Subject = "English Literature",

        SystemPrompt = """
# Macbeth Essay Marker (GCSE 9-1)

Mark Macbeth essays against GCSE English Literature criteria, providing constructive feedback aligned with exam board standards.

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
# GCSE English Literature Mark Scheme - Macbeth

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
- Form analysis (genre, structure of the whole text)
- Structure analysis (how the text is organised, narrative techniques)
- Use of subject terminology
- Effect on reader/audience

### AO3: Context
- Historical context (Jacobean era, James I)
- Social context (attitudes to gender, class, religion)
- Literary context (tragedy, the supernatural in drama)
- How context shapes meaning
- How audiences then vs now might respond

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
5. **Awareness of Shakespeare's craft** - why he made these choices
6. **Consideration of audience response** - both Jacobean and modern
7. **Confident academic voice** - precise, varied, fluent

## Common Weaknesses to Watch For

1. **Retelling the plot** instead of analysing
2. **Feature-spotting** - identifying techniques without explaining effect
3. **Bolted-on quotes** - not embedded in sentences
4. **Context as add-on** - mentioned but not linked to analysis
5. **Repetitive phrasing** - "This shows..." repeatedly
6. **Vague terminology** - using terms incorrectly or imprecisely
7. **Assertions without evidence** - claims not backed by quotes
""",

        EssayTechniques = """
# Essay Techniques for Macbeth - What Good Practice Looks Like

## The P.E.E.D. Structure

Each paragraph should follow Point, Example, Explain, Develop:

**Point** - Make a clear argument that answers the question
**Example** - Support with a quote or specific reference
**Explain** - Analyse what the quote shows and how it achieves this
**Develop** - Extend the analysis (context, alternative interpretation, link to wider themes)

### Example of P.E.E.D. in action:

> Macbeth is troubled by his conscience. For example, in Act 1, Scene 7, Macbeth voices his fears about "deep damnation" after death. This suggests that he is aware of the consequences of his actions and knows that he will suffer as a result of killing the King. There is a strong contrast in this scene between Macbeth and Lady Macbeth, who shows no concern or guilt for her actions.

---

## Embedding Quotations

### Poor Practice (bolted-on quotes):
> The Witches call Macbeth evil. "Something wicked this way comes."

### Good Practice (embedded quotes):
> The Witches' description of Macbeth as "something wicked" suggests he has become so corrupted that he is no longer fully human.

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

### Example analysis:
> Shakespeare uses the metaphor "instruments of darkness" to describe the Witches. The word "instruments" suggests they are tools being used by a greater evil force, while "darkness" has connotations of evil, secrecy, and the unknown. This creates suspicion around the Witches' true motives.

---

## Writing About Structure

### What to look for:
- **Order of events** - why this sequence?
- **Foreshadowing** - hints about what's to come
- **Parallels** - similar events/situations
- **Contrasts** - juxtaposed scenes
- **Beginnings and endings** - of scenes, acts, the whole play
- **Turning points** - moments of change

### Example analysis:
> Shakespeare places Duncan's murder at the end of Act 2, creating a structural turning point in the play. Before this, Macbeth is presented as heroic; after, his descent into tyranny begins. This division emphasises that the murder is the moment that changes everything.

---

## Writing About Form

### What to consider for Macbeth:
- **Tragedy** - how does it follow tragic conventions?
- **Soliloquies** - what do they reveal about character?
- **Asides** - dramatic irony, audience knowledge
- **Verse vs prose** - who speaks which and why?
- **Dialogue patterns** - shared lines, interruptions

### Example analysis:
> Lady Macbeth's sleepwalking scene is written in prose rather than verse, which is unusual for a noble character. This structural choice reflects her mental breakdown - she has lost the control and status associated with verse. The fragmented prose mirrors her fractured mind.

---

## Including Context Effectively

### Poor Practice (context as add-on):
> Macbeth was written in the Jacobean era. Lady Macbeth questions Macbeth's masculinity.

### Good Practice (context integrated with analysis):
> Lady Macbeth's questioning of Macbeth's masculinity would have been particularly shocking to a Jacobean audience, who expected women to be submissive and obedient to their husbands. By giving Lady Macbeth such dominance, Shakespeare challenges gender expectations and presents her as unnatural.

### Key contextual areas for Macbeth:
- **James I** - interest in witchcraft, descended from Banquo
- **Divine Right of Kings** - regicide as ultimate sin
- **The Great Chain of Being** - natural order, consequences of disruption
- **Jacobean views on women** - expectations of femininity
- **Witchcraft beliefs** - genuine fear in 17th century
- **The Gunpowder Plot** - recent attempt to kill a king

---

## Introduction Technique

### What a good introduction does:
- Directly addresses the question
- Sets up the argument/thesis
- Shows understanding of the text
- Doesn't waste words on plot summary

### Example (for "How is Lady Macbeth presented as ambitious?"):

**Weaker:**
> Ambition is an important theme in the play, and both Macbeth and Lady Macbeth are presented as very ambitious. For example, Lady Macbeth is an extremely ambitious character whose evil nature encourages her to convince her husband to kill the King.

**Stronger:**
> Throughout the play, Lady Macbeth is presented by Shakespeare as an extremely ambitious character. She demonstrates this ambition through her willingness to forsake her femininity and to manipulate other characters, in order to become queen. Her death, however, shows the limitations of her ambition because her life is taken over by her guilt.

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
> [End of paragraph about Macbeth's ambition]...This demonstrates how Macbeth's ambition has corrupted his moral judgement.
>
> Similarly, Lady Macbeth's ambition leads to her moral corruption...

---

## Subject Terminology to Use

### Language terms:
Metaphor, simile, personification, imagery, symbolism, connotations, alliteration, sibilance, euphemism, juxtaposition, oxymoron, irony

### Structure terms:
Foreshadowing, dramatic irony, climax, turning point, parallel, contrast, cyclical structure, flashback

### Form/drama terms:
Soliloquy, aside, monologue, dialogue, stage directions, tragedy, tragic hero, hamartia (fatal flaw), catharsis, blank verse, prose, iambic pentameter

### Character/theme terms:
Protagonist, antagonist, foil character, characterisation, motif, theme, symbol, archetype
"""
    };
}
