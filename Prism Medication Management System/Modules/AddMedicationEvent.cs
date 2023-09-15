using Prism.Events;
using Prism_Medication_Management_System.Models;

namespace Prism_Medication_Management_System.Modules
{
    internal class AddMedicationEvent : PubSubEvent<Medication>
    {
    }
}
