using Medical.Models;

namespace Medical.Services;

public class GuidanceService
{
    public string GetTodayGuidance(AgeGroup group, bool bangla) => (group, bangla) switch
    {
        (AgeGroup.Under18, false) => "Eat on time, drink water and sleep 9 hours. Your guardian will get your reminders.",
        (AgeGroup.Under18, true) => "সময়মতো খাবার খান, পানি পান করুন এবং ৯ ঘণ্টা ঘুমান। আপনার অভিভাবক রিমাইন্ডার পাবেন।",

        (AgeGroup.Age18To39, false) => "Walk 30 minutes today and drink 8 glasses of water. Plan your yearly checkup.",
        (AgeGroup.Age18To39, true) => "আজ ৩০ মিনিট হাঁটুন এবং ৮ গ্লাস পানি পান করুন। বছরে একবার চেকআপের পরিকল্পনা করুন।",

        (AgeGroup.Age40To49, false) => "Check your blood pressure and sugar this month. Walk 20 minutes and eat less salt.",
        (AgeGroup.Age40To49, true) => "এই মাসে রক্তচাপ ও ব্লাড সুগার পরীক্ষা করুন। ২০ মিনিট হাঁটুন এবং লবণ কম খান।",

        (AgeGroup.Age50Plus, false) => "Take your morning medicine after breakfast. Walk 15 minutes. Tap Home Nursing for a checkup at home.",
        (AgeGroup.Age50Plus, true) => "সকালের নাস্তার পর ওষুধ খান। ১৫ মিনিট হাঁটুন। বাসায় চেকআপের জন্য হোম নার্সিং চাপুন।",

        (_, false) => "Stay active and drink enough water today.",
        (_, true) => "আজ সক্রিয় থাকুন এবং পর্যাপ্ত পানি পান করুন।"
    };
}