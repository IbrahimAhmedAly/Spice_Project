using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spice.Utiltiy
{
    public class StripeSettings
    {
        // notice : to add stripe you should go to appsetting.json first and declare it next go to manage NuGet package and install "Stripe.net" then make class like this then go to class Startup to configure stripe not forget to add down also.
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
