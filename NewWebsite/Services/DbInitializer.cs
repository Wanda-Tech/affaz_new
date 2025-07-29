using Microsoft.AspNetCore.Identity;
using NewWebsite.Helpers;

public static class DbInitializer
{
    public static void Seed(NewsWebsiteContext context)
    {
        if (!context.NewsCategories.Any())
        {
            context.NewsCategories.AddRange(
                new NewsCategory { Name = "Articles" },
                new NewsCategory { Name = "Reports" },
                new NewsCategory { Name = "Breaking" }
            );
            context.SaveChanges();
        }


        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new Role { Name = "Admin" },
                new Role { Name = "Editor" }
            );
            context.SaveChanges();
        }

        User? userAdmin = context.Users.FirstOrDefault(u => u.Email == "admin@news.com");

        // Seed user
        if (!context.Users.Any())
        {
            userAdmin = new User
            {
                Phone = "77787812",
                Email = "admin@news.com",
            };

            string password = PasswordHasher.HashPassword(userAdmin.Email);

            userAdmin.Password = password;

            var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");


            userAdmin.UserRoles.Add(new UserRole
            {
                Role = adminRole
            });

            context.SaveChanges();
        }


        // Seed news
        if (!context.News.Any())
        {
            var allNewsCategories = context.NewsCategories
                .Select(q => q.Id).ToList();
            var images = new[] {
            "https://www.reuters.com/resizer/v2/KALBIIWFZBNBVO3JFREMRRNUOM.jpg?auth=bdd8dbc1b6d1f5e97dc4befa011565e35423ffa58f35ce76fcd198d36c8c5c66&width=1200&quality=80",
            "https://www.reuters.com/resizer/v2/UQC7RFFQYVNIFOKQTRCKFYZNVI.jpg?auth=4d10655282043c200d3771553d5c58f92105caeb3ac615fea36557be5c2767a8&width=1200&quality=80",
            "https://www.reuters.com/resizer/v2/N7XHIIYNANIVLDUB53UV3FRIWM.jpg?auth=2297ef2e78c1e96344542adfa2380390d15b702bb66c95485b2c1f7d8dd0bd47&width=1200&quality=80",
            "https://www.reuters.com/resizer/v2/CO2INK6Y5ZKSPMQFLHKKA3NXME.jpg?auth=9df15600a71bd9f7544157119fb85437f3e2ffd28b66cd22fd53b775e1364ef1&width=1200&quality=80"
            };

            var titles = new[] {
            "Thailand F-16 jet bombs Cambodian targets as border clash escalates",
            "Big Alcohol prepares to fight back as buzzy cannabis drinks steal sales",
            "Hyundai Motor warns of bigger hit from US tariffs after second-quarter profit fall",
            "Green hydrogen retreat poses threat to emissions targets",
            "'Japanese First' party emerges as election force with tough immigration talk"
            };
            

            Random rnd = new Random();

            for (int i = 0; i < 20; i++)
            {
                context.News.Add(
                    new News
                    {

                        // NewsCategory = context.NewsCategories.FirstOrDefault(nc => nc.Name == "Articles"),
                        NewsCategoryId = allNewsCategories[rnd.Next() % allNewsCategories.Count],
                        User = userAdmin,
                        CreatedDate = DateTime.UtcNow,

                        NewsStatus = NewsStatus.Draft,

                        Title = titles[rnd.Next() % titles.Length],
                        Summary = @"<ul>
                        <li>Sanseito, birthed on YouTube, makes election gains</li>
                        <li>Party has also pledged tax cuts and welfare spending</li>
                        <li>Leader says he wants to expand lower house presence</li>
                    </ul>",
                    ImageUrl = images[rnd.Next() % images.Length],

                        Content = @"
            <p>
                TOKYO, July 21 (Reuters) - The fringe far-right Sanseito party emerged as one of the biggest winners in
                Japan's upper house election on Sunday, gaining support with warnings of a 'silent invasion' of
                immigrants, and pledges for tax cuts and welfare spending.</p>

            <p>
                Birthed on YouTube during the COVID-19 pandemic spreading conspiracy theories about vaccinations and a
                cabal of global elites, the party broke into mainstream politics with its ""Japanese First"" campaign.</p>

            <p> ""We were criticized as being xenophobic and discriminatory. The public came to understand that the media
                was wrong and Sanseito was right,"" Kamiya said.</p>

            <p>Kamiya's message grabbed voters frustrated with a weak economy and currency that has lured tourists in
                record numbers in recent years, further driving up prices that Japanese can ill afford, political
                analysts say.</p>

            <p>Japan's fast-ageing society has also seen foreign-born residents hit a record of about 3.8 million last
                year, though that is just 3% of the total population, a fraction of the corresponding proportion in the
                United States and Europe.</p>

            <h4>INSPIRED BY TRUMP</h4>

            <p>Kamiya, a former supermarket manager and English teacher, told Reuters before the election that he had
                drawn inspiration from U.S. President Donald Trump's ""bold political style"".</p>

            <p>
                He has also drawn comparisons with Germany's AfD and Reform UK although right-wing populist policies
                have yet to take root in Japan as they have in Europe and the United States.</p>

            <p>
                Post-election, Kamiya said he plans to follow the example of Europe's emerging populist parties by
                building alliances with other small parties rather than work with an LDP administration, which has ruled
                for most of Japan's postwar history.</p>",

                    }
                );
            }



            context.SaveChanges();
        }
    }

}