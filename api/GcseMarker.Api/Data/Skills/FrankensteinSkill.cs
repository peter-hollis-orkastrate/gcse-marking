namespace GcseMarker.Api.Data.Skills;

public static class FrankensteinSkill
{
    public static SkillDefinition Definition { get; } = new()
    {
        Id = "frankenstein",
        Name = "AQA - English Literature - Frankenstein",
        Description = "GCSE English Literature essay marker for Frankenstein, aligned with Grade 9-1 marking criteria. Use when asked to mark, assess, grade, or provide feedback on a Frankenstein essay, or when a user uploads an essay about Frankenstein for evaluation. Provides band-level assessment (4-5, 6-7, 8-9), detailed feedback against official criteria, and specific improvement suggestions.",
        Subject = "English Literature",

        SystemPrompt = """
# Frankenstein Essay Marker (GCSE 9-1)

Mark Frankenstein essays against GCSE English Literature criteria, providing constructive feedback aligned with exam board standards.

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
# GCSE English Literature Mark Scheme - Frankenstein

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
- Historical context (Romantic era, Industrial Revolution, scientific advances)
- Social context (class, gender roles, attitudes to outsiders)
- Biographical context (Shelley's life, family, losses)
- Literary context (Gothic genre, Romantic poetry, Paradise Lost)
- Scientific context (galvanism, Enlightenment, "playing God")
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
5. **Awareness of Shelley's craft** - why she made these choices
6. **Consideration of reader response** - both Romantic-era and modern
7. **Confident academic voice** - precise, varied, fluent
8. **Engagement with narrative complexity** - multiple narrators, unreliability

## Common Weaknesses to Watch For

1. **Retelling the plot** instead of analysing
2. **Feature-spotting** - identifying techniques without explaining effect
3. **Bolted-on quotes** - not embedded in sentences
4. **Context as add-on** - mentioned but not linked to analysis
5. **Repetitive phrasing** - "This shows..." repeatedly
6. **Vague terminology** - using terms incorrectly or imprecisely
7. **Assertions without evidence** - claims not backed by quotes
8. **Simplistic sympathy** - only seeing the Creature as victim or only as monster
9. **Ignoring narrative framing** - forgetting who is telling each part of the story
""",

        EssayTechniques = """
# Essay Techniques for Frankenstein - What Good Practice Looks Like

## The P.E.E.D. Structure

Each paragraph should follow Point, Example, Explain, Develop:

**Point** - Make a clear argument that answers the question
**Example** - Support with a quote or specific reference
**Explain** - Analyse what the quote shows and how it achieves this
**Develop** - Extend the analysis (context, alternative interpretation, link to wider themes)

### Example of P.E.E.D. in action:

> Shelley presents Victor as morally responsible for the Creature's suffering. When Victor describes his creation as a "catastrophe," he immediately distances himself from any parental duty. The word "catastrophe" frames the Creature as a disaster rather than a being deserving of care, revealing Victor's refusal to accept responsibility. This abandonment reflects contemporary debates about parental duty and can be linked to Shelley's own experience of maternal loss and absent parents.

---

## Embedding Quotations

### Poor Practice (bolted-on quotes):
> The Creature is rejected by society. "I am malicious because I am miserable."

### Good Practice (embedded quotes):
> The Creature's declaration that he is "malicious because I am miserable" directly links his violent actions to Victor's neglect, forcing readers to question who the true monster is.

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
- **Gothic vocabulary** - darkness, horror, sublime
- **Romantic language** - nature, emotion, the sublime
- **Biblical/literary allusions** - Paradise Lost, Prometheus

### Example analysis:
> Shelley uses the simile "like one who treads over the bodies of the dead" to describe Victor's mental state. The image of walking over corpses suggests both guilt and desensitisation—Victor is haunted yet continues on his destructive path. The biblical connotations of death and judgement foreshadow the tragedy to come.

---

## Writing About Structure

### What to look for:
- **Frame narrative** - Walton's letters containing Victor's story containing the Creature's story
- **Order of events** - why this sequence?
- **Foreshadowing** - hints about what's to come
- **Parallels** - between Victor, Walton, and the Creature
- **Contrasts** - juxtaposed scenes and settings
- **Beginnings and endings** - how chapters/volumes open and close
- **The Creature's central narrative** - positioned at the heart of the novel

### Example analysis:
> Shelley places the Creature's narrative at the structural centre of the novel, surrounded by Victor's account, which is itself framed by Walton's letters. This "Chinese box" structure forces readers to encounter the Creature's perspective only after forming judgements based on Victor's biased account, compelling us to reassess our sympathies.

---

## Writing About Form

### What to consider for Frankenstein:
- **Gothic novel** - atmosphere, horror, the supernatural, isolated settings
- **Epistolary elements** - Walton's letters to his sister
- **Multiple narrators** - Walton, Victor, the Creature
- **Unreliable narration** - whose account can we trust?
- **The Romantic sublime** - awe, terror, nature's power
- **Science fiction elements** - early example of the genre

### Example analysis:
> Shelley's use of multiple narrators prevents readers from accepting any single perspective as truth. Victor describes the Creature as a "daemon," yet when the Creature speaks for himself, he is eloquent and sympathetic. This narrative ambiguity reflects the novel's central question: who is the real monster? Shelley refuses to provide a simple answer.

---

## Including Context Effectively

### Poor Practice (context as add-on):
> Frankenstein was written during the Romantic period. Victor creates a monster and then abandons it.

### Good Practice (context integrated with analysis):
> Victor's obsessive pursuit of knowledge reflects Romantic-era anxieties about scientific progress. Shelley was writing in the aftermath of galvanism experiments that suggested electricity could animate dead tissue, and her portrayal of Victor's "workshop of filthy creation" questions whether scientists have the moral right to pursue knowledge regardless of consequences—a debate that remains urgent today.

### Key contextual areas for Frankenstein:
- **The Romantic movement** - emphasis on emotion, nature, individual experience
- **Scientific advances** - galvanism, Erasmus Darwin's experiments, Enlightenment rationalism
- **Industrial Revolution** - fears about technology and dehumanisation
- **Shelley's biography** - mother's death, father's influence, loss of children, Percy Shelley
- **Paradise Lost** - the Creature as both Adam and Satan, Victor as failed God
- **The Prometheus myth** - subtitle "The Modern Prometheus," fire as knowledge/punishment
- **Gender and creation** - male usurpation of female reproductive power
- **Social outcasts** - treatment of the "other," disability, appearance-based prejudice
- **The 1818 vs 1831 editions** - changes Shelley made and what they reveal

---

## Introduction Technique

### What a good introduction does:
- Directly addresses the question
- Sets up the argument/thesis
- Shows understanding of the text
- Doesn't waste words on plot summary

### Example (for "How does Shelley present the Creature as a sympathetic character?"):

**Weaker:**
> In Frankenstein, the Creature is a monster created by Victor Frankenstein. Shelley presents the Creature as sympathetic because he is rejected by society and his creator.

**Stronger:**
> Shelley systematically constructs sympathy for the Creature through his eloquent first-person narrative, his education through benevolent literature, and the repeated cruelty he suffers from humans. By positioning the Creature's account at the novel's structural centre—and by drawing explicit parallels with Milton's Satan—Shelley forces readers to question whether monstrosity is inherent or created by society's rejection.

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
> [End of paragraph about Victor's isolation]...This self-imposed solitude ultimately contributes to his destruction.
>
> Similarly, the Creature's isolation is presented as deeply damaging, though crucially his loneliness is imposed by others rather than chosen...

---

## Subject Terminology to Use

### Language terms:
Metaphor, simile, personification, imagery, symbolism, connotations, alliteration, pathetic fallacy, juxtaposition, irony, allusion, hyperbole, the sublime

### Structure terms:
Frame narrative, foreshadowing, dramatic irony, climax, turning point, parallel, contrast, cyclical structure, embedded narrative, chronological disruption

### Form/narrative terms:
Gothic novel, epistolary novel, first-person narrator, unreliable narrator, multiple perspectives, Romantic literature, science fiction, narrative framing

### Character/theme terms:
Protagonist, antagonist, doppelganger/double, foil character, characterisation, motif, theme, symbol, archetype, Byronic hero, tragic hero, hubris

## Key Symbols and Motifs to Analyse

- **Fire/light** - knowledge, danger, Prometheus, enlightenment and destruction
- **Ice/Arctic** - isolation, inhospitality, the sublime, emotional coldness
- **Nature** - the sublime, healing power, contrast to unnatural creation
- **The moon** - creation scenes, Gothic atmosphere, cyclical time
- **Books/education** - Paradise Lost, Plutarch, Sorrows of Werter—the Creature's moral formation
- **Eyes** - "watery eyes," judgement, the soul, horror at first sight
- **Birth/creation imagery** - "workshop of filthy creation," unnatural birth
- **Doubles/mirrors** - Victor and the Creature as reflections of each other
"""
    };
}
