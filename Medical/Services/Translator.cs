using System.Globalization;
using Medical.Models;

namespace Medical.Services;

public class Translator
{
    public bool IsBangla => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "bn";

    private static readonly Dictionary<string, (string En, string Bn)> Map = new()
    {
        ["AppName"] = ("ShasthoSheba", "স্বাস্থ্যসেবা"),
        ["Tagline"] = ("Verified doctors, home nurse and daily health guidance in one place.",
                              "যাচাইকৃত ডাক্তার, হোম নার্স এবং দৈনিক স্বাস্থ্য নির্দেশনা এক জায়গায়।"),
        ["Home"] = ("Home", "হোম"),
        ["FindDoctor"] = ("Find Doctor", "ডাক্তার খুঁজুন"),
        ["Login"] = ("Login", "লগইন"),
        ["Register"] = ("Create account", "নতুন অ্যাকাউন্ট"),
        ["Logout"] = ("Logout", "লগআউট"),
        ["FullName"] = ("Full name", "পুরো নাম"),
        ["Phone"] = ("Mobile number", "মোবাইল নম্বর"),
        ["EmailOptional"] = ("Email (optional)", "ইমেইল (ঐচ্ছিক)"),
        ["Password"] = ("Password", "পাসওয়ার্ড"),
        ["PhoneOrEmail"] = ("Mobile number or email", "মোবাইল নম্বর বা ইমেইল"),
        ["RememberMe"] = ("Remember me", "আমাকে মনে রাখুন"),
        ["NoAccount"] = ("No account? Create one", "অ্যাকাউন্ট নেই? তৈরি করুন"),
        ["HaveAccount"] = ("Already have an account? Login", "আগে থেকেই অ্যাকাউন্ট আছে? লগইন করুন"),
        ["InvalidLogin"] = ("Wrong mobile/email or password.", "মোবাইল/ইমেইল বা পাসওয়ার্ড ভুল।"),

        ["AgeTitle"] = ("Welcome! How old are you?", "স্বাগতম! আপনার বয়স কত?"),
        ["AgeSub"] = ("We use this to set up your daily plan.", "আপনার দৈনিক পরিকল্পনা সাজাতে আমরা এটি ব্যবহার করি।"),
        ["Under18"] = ("Under 18", "১৮ এর নিচে"),
        ["Age18To39"] = ("18 - 39", "১৮ - ৩৯"),
        ["Age40To49"] = ("40 - 49", "৪০ - ৪৯"),
        ["Age50Plus"] = ("50 +", "৫০ +"),
        ["HomeNurseAvail"] = ("Home nurse available", "হোম নার্স পাওয়া যায়"),
        ["Continue"] = ("Continue", "এগিয়ে যান"),

        ["Hello"] = ("Hello", "হ্যালো"),
        ["NextVisit"] = ("Next visit", "পরবর্তী ভিজিট"),
        ["NoVisit"] = ("No upcoming visit", "কোনো ভিজিট নেই"),
        ["Medicines"] = ("Medicines", "ওষুধ"),
        ["CareAtHome"] = ("Care at home", "বাসায় সেবা"),
        ["NurseVisit"] = ("Nurse visit", "নার্স ভিজিট"),
        ["TopDoctors"] = ("Top doctors for you", "আপনার জন্য সেরা ডাক্তার"),
        ["TodayGuidance"] = ("Today's guidance", "আজকের নির্দেশনা"),

        ["Specialties"] = ("Choose a specialty", "বিভাগ বেছে নিন"),
        ["TopRated"] = ("Top rated", "সেরা রেটিং"),
        ["FastReply"] = ("Fast reply", "দ্রুত উত্তর"),
        ["Experienced"] = ("Experienced", "অভিজ্ঞ"),
        ["Fee"] = ("Fee", "ফি"),
        ["TopPick"] = ("Top Pick", "টপ পিক"),
        ["FastReplier"] = ("Fast Replier", "দ্রুত উত্তরদাতা"),
        ["BookChat"] = ("Book / Chat", "বুক / চ্যাট"),
        ["RepliesIn"] = ("Replies in ~{0} min", "প্রায় {0} মিনিটে উত্তর"),
        ["YearsExp"] = ("{0} yrs experience", "{0} বছরের অভিজ্ঞতা"),
        ["Reviews"] = ("reviews", "রিভিউ"),
        ["NoDoctors"] = ("No verified doctors in this specialty yet.", "এই বিভাগে এখনো কোনো যাচাইকৃত ডাক্তার নেই।"),

        ["Today"] = ("Today", "আজ"),
        ["Visits"] = ("visits", "ভিজিট"),
        ["AvgReply"] = ("Avg reply", "গড় উত্তর"),
        ["Rating"] = ("Rating", "রেটিং"),
        ["Waiting"] = ("Waiting", "অপেক্ষায়"),
        ["PatientQueue"] = ("Patient queue", "রোগীর সিরিয়াল"),
        ["History"] = ("History", "হিস্টোরি"),
        ["MinShort"] = ("min", "মিনিট"),
        ["GoodDay"] = ("Welcome", "স্বাগতম"),
        ["QuickReplyTip"] = ("Quick replies improve your ranking.", "দ্রুত উত্তর দিলে আপনার র‍্যাঙ্কিং ভালো হয়।"),
        ["NoWaiting"] = ("No patients waiting.", "কোনো রোগী অপেক্ষায় নেই।"),
    };

    public string this[string key] =>
        Map.TryGetValue(key, out var v) ? (IsBangla ? v.Bn : v.En) : key;

    public string Format(string key, params object[] args) => string.Format(this[key], args);

    public string AgeGroupLabel(AgeGroup g) => g switch
    {
        AgeGroup.Under18 => this["Under18"],
        AgeGroup.Age18To39 => this["Age18To39"],
        AgeGroup.Age40To49 => this["Age40To49"],
        AgeGroup.Age50Plus => this["Age50Plus"],
        _ => "-"
    };
}