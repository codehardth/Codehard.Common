// namespace Codehard.Functional.AspNetCore;
//
// public interface HttpResponsable<F> 
//     where F : HttpResponsable<F>
// {
//     public static abstract K<F, B> Map<A, B>(K<F, A> fa, Func<A, B> f);
// }
//
// public static class HttpResponsableExtensions
// {
//     public static K<F, B> Select<E, A, B>(this K<E, A> fa, Func<A, B> f)
//         where F : HttpResponsable<E>
//         where A : IActionResult
//         where B : IActionResult =>
//         F.Select(fa, f);
// }