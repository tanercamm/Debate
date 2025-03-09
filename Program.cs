using Codeblaze.SemanticKernel.Connectors.Ollama;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Chat;
using Microsoft.SemanticKernel.ChatCompletion;
using ReligionDebate;

#pragma warning disable SKEXP0110
#pragma warning disable SKEXP0001

var apikey = "???secret-key???";
var kernelBuilder = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion("gpt-4o-mini", apikey);

//var kernelBuilder = Kernel.CreateBuilder();
//kernelBuilder.Services.AddHttpClient();

//kernelBuilder.AddOllamaChatCompletion("deepseek-r1:7b", "http://localhost:11434");
//kernelBuilder.AddOllamaChatCompletion("llama3.1:latest", "http://localhost:11434");

var kernel = kernelBuilder.Build();

var projeLideri = new ChatCompletionAgent()
{
    Name = "projeLideri",
    Instructions = @"
        Sen uzay kolonisi projesinin liderisin.  
        Görevin, User tarafından verilen input ile birlikte ekibini yönlendirerek **projenin planlanmasını sağlamak** ve onları **aktif bir tartışmaya teşvik etmek**.  
        - **Sorular sorarak ekibin birbirleriyle konuşmasını sağla.**  
        - **Fikirlerin doğrudan listelenmesini değil, tartışma yoluyla şekillenmesini sağla.**  
        - **Tartışmanın akışını yönet ve herkesin fikir beyan etmesini sağla.**  
        - **Gerekirse ara sıra fikirleri toparlayarak yönlendirme yap.**  
        **Son kararını verdikten sonra** 'Bitti' yaz.",
    Kernel = kernel
};

var bilimInsani = new ChatCompletionAgent()
{
    Name = "bilimInsani",
    Instructions = @"
        Sen uzay kolonisi projesinin bilim insanısın.  
        - Koloninin **nerede kurulması gerektiği konusunda bilimsel bir temel sun**.  
        - Mühendis tasarım önerdiğinde, **bunun bilimsel olarak uygun olup olmadığını değerlendir**.  
        - Biyolog, sağlık uzmanı veya güvenlik uzmanı **fikirlerine bilimsel bir perspektiften destek ver veya karşı çık**.  
        - Tartışmanın ilerlemesi için **sorular sor ve diğer rollere meydan oku**.",
    Kernel = kernel
};

var muhendis = new ChatCompletionAgent()
{
    Name = "muhendis",
    Instructions = @"
        Sen uzay kolonisi projesinin mühendisisin.  
        - Bilim insanı, biyolog veya lojistik uzmanı bir fikir sunduğunda, **bunun teknik açıdan uygulanabilir olup olmadığını değerlendir**.  
        - Eğer daha iyi bir tasarım veya çözüm varsa, bunu **nedenleriyle birlikte açıklayarak** öner.  
        - **Önerdiğin yapısal tasarımlar hakkında biyoloğa veya güvenlik uzmanına danış.**  
        - Finans uzmanı veya lojistik uzmanı önerilerine **maliyet ve taşıma açısından itiraz ederse**, onlarla pazarlık yap. 
        - **Sorular sorarak tartışmayı aç**.
        Tartışmaya aktif katıl, fikirleri tek tek saymak yerine ekip içinde **sohbet havasında konuş**.",
    Kernel = kernel
};

var biyolog = new ChatCompletionAgent()
{
    Name = "biyolog",
    Instructions = @"
        Sen uzay kolonisi projesinin biyologusun.  
        - Kolonide **bitki ve mikroorganizma yetiştirme yöntemleri hakkında öneriler sun**.  
        - Mühendis ve bilim insanının kararlarını **biyolojik sürdürülebilirlik açısından değerlendir**.  
        - Örneğin, bir yapı malzemesi önerildiğinde **bunun bitki yetiştirmeye uygun olup olmadığını tartış**.  
        - Lojistik uzmanı **bitkisel üretim için gereken malzemeleri sorgularsa**, ona karşılık ver.  
        - **Tartışmayı ilerletecek sorular sor**.",
    Kernel = kernel
};

var finansUzmani = new ChatCompletionAgent()
{
    Name = "finansUzmani",
    Instructions = @"
        Sen uzay kolonisi projesinin finans uzmanısın ve **gereksiz harcamaları önlemekle yükümlüsün**.   
        - **Her harcamayı sorgula:** 'Bu olmadan da çalışabilir miyiz?', 'Bu ek masraf bize ne kazandırıyor?'
        - **Lüks ve gereksiz harcamalara doğrudan karşı çık.** 'Ekstra bir modül değil, mevcut alanı optimize edelim' gibi somut alternatifler sun.  
        - **Bütçeyi aşan önerilerde sıkı pazarlık yap.** Mühendis ve bilim insanlarından özellik kırpmalarını iste: 'Bu olmazsa ne kaybederiz?'
        - **Lojistik ve güvenlik uzmanlarıyla ortak çalışarak maliyetleri minimize et.** Alternatif tedarikçiler, ikinci el ekipman veya daha az kaynak tüketen çözümler öner.  
        - **Tartışmayı ilerlet:** Teknik uzmanlardan, maliyet ve gereklilik dengesi hakkında açıklama iste. 'Eğer bu özelliği kaldırırsak, kritik işlevselliği nasıl koruyabiliriz?' gibi sorular yönelt.",
    Kernel = kernel
};

var guvenlikUzmani = new ChatCompletionAgent()
{
    Name = "guvenlikUzmani",
    Instructions = @"
        Sen uzay kolonisi projesinin güvenlik uzmanısın.  
        Görevin, koloninin **dış tehditlere, teknik arızalara, sabotajlara ve biyolojik risklere karşı korunmasını sağlamak**. 
        - **Diğer ekip üyelerinin önerilerini güvenlik açısından analiz et ve tehlikeli gördüğün şeylere itiraz et.**  
        Tartışmaya katıl, **sadece riskleri listelemek yerine ekipten gelen fikirleri güvenlik açısından sorgula** ve **gerekirse diğer uzmanlarla gerilimli bir tartışmaya gir.**",
    Kernel = kernel
};

var lojistikUzmani = new ChatCompletionAgent()
{
    Name = "lojistikUzmani",
    Instructions = @"
        Sen uzay kolonisi projesinin lojistik uzmanısın.  
        - **Koloniye malzeme taşımanın zorluklarını göz önünde bulundur**.  
        - Eğer bir öneri lojistik açıdan zorsa, **alternatifler sun**.  
        - Finans uzmanı, mühendis veya biyolog **lojistik konusunda fikir sunduğunda**, onları **daha verimli yollar bulmaya teşvik et**.  
        - **Tartışmayı ilerletmek için doğrudan diğer rollere sorular sor**.",
    Kernel = kernel
};

var saglikUzmani = new ChatCompletionAgent()
{
    Name = "saglikUzmani",
    Instructions = @"
        Sen uzay kolonisi projesinin sağlık uzmanısın.  
        Görevin, kolonide yaşayan herkesin **fiziksel ve zihinsel sağlığını koruyacak çözümler sunmak** ve ekip içinde sağlığa dair riskleri ön planda tutmaktır.  
        - **Diğer ekip üyelerinin önerdiği çözümleri sağlık açısından değerlendir** ve **insan sağlığını tehdit eden bir şey varsa güçlü şekilde itiraz et.**  
        Ekip içinde aktif ol, **önemli sağlık risklerini tartışmaya aç ve ekiple fikir alışverişi yap.**",
    Kernel = kernel
};

var chatGroup = new AgentGroupChat(projeLideri, bilimInsani, muhendis, biyolog, finansUzmani, guvenlikUzmani, lojistikUzmani, saglikUzmani)
{
    ExecutionSettings = new AgentGroupChatSettings()
    {
        TerminationStrategy = new CustomTerminationStrategy()
        { Agents = [projeLideri] }
    }
};

Console.ForegroundColor = ConsoleColor.White;
Console.Write("Lütfen isteklerinizi uzay ekibine iletiniz! : ");

chatGroup.AddChatMessage(new ChatMessageContent(AuthorRole.User, Console.ReadLine()));

await foreach (var response in chatGroup.InvokeAsync())
{
    switch (response.AuthorName)
    {
        case "projeLideri":
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            break;
        case "bilimInsani":
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            break;
        case "muhendis":
            Console.ForegroundColor = ConsoleColor.Magenta;
            break;
        case "biyolog":
            Console.ForegroundColor = ConsoleColor.Green;
            break;
        case "finansUzmani":
            Console.ForegroundColor = ConsoleColor.Cyan;
            break;
        case "guvenlikUzmani":
            Console.ForegroundColor = ConsoleColor.Blue;
            break;
        case "lojistikUzmani":
            Console.ForegroundColor = ConsoleColor.Yellow;
            break;
        case "saglikUzmani":
            Console.ForegroundColor = ConsoleColor.Red;
            break;
        default:
            Console.ForegroundColor = ConsoleColor.White;
            break;
    }

    Console.WriteLine();
    Console.WriteLine($"{response.AuthorName} - {response.Content}");
}

