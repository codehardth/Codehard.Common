using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace Codehard.Functional.EntityFramework.Monads;

public record DbEnv(DbContext Context);

public record Db<A>(ReaderT<DbEnv, IO, A> RunDb) : K<Db, A>
{
    public Db<B> Select<B>(Func<A, B> m) => this.Kind().Select(m).As();

    public Db<C> SelectMany<B, C>(Func<A, K<Db, B>> b, Func<A, B, C> p) => this.Kind().SelectMany(b, p).As();

    public Db<C> SelectMany<B, C>(Func<A, K<IO, B>> b, Func<A, B, C> p) => this.Kind().SelectMany(b, p).As();

    public static Db<A> operator |(Db<A> ma, Db<A> mb) =>
        ma.Catch(mb).As();

    public static Db<A> operator |(Db<A> ma, CatchM<Error, Db, A> mb) =>
        ma.Catch(mb).As();

    public static Db<A> operator |(Db<A> ma, Pure<A> mb) =>
        ma.Catch(mb).As();

    public static Db<A> operator |(Db<A> ma, Error mb) =>
        ma.Catch(mb).As();
}

public abstract partial class Db : Monad<Db>, Readable<Db, DbEnv>, Fallible<Db>
{
    public static K<Db, B> Map<A, B>(Func<A, B> mapFunc, K<Db, A> db)
        => new Db<B>(db.As().RunDb.Map(mapFunc).As());

    public static K<Db, A> Pure<A>(A value)
        => new Db<A>(ReaderT<DbEnv, IO, A>.Pure(value));

    public static K<Db, B> Apply<A, B>(K<Db, Func<A, B>> applyFunc, K<Db, A> db)
        => new Db<B>(applyFunc.As().RunDb.Apply(db.As().RunDb));

    public static K<Db, B> Bind<A, B>(K<Db, A> db, Func<A, K<Db, B>> bindFunc)
        => new Db<B>(db.Run().Bind(x => bindFunc(x).Run()).As());

    public static K<Db, A> Asks<A>(Func<DbEnv, A> f)
        => new Db<A>(ReaderT.asks<IO, A, DbEnv>(f));

    public static K<Db, A> Local<A>(Func<DbEnv, DbEnv> f, K<Db, A> db)
        => new Db<A>(ReaderT.local(f, db.Run().As()));

    public static K<Db, A> LiftIO<A>(IO<A> ma)
        => new Db<A>(ReaderT.liftIO<DbEnv, IO, A>(ma));

    public static Db<A> Lift<A>(IO<A> io)
        => new(ReaderT<DbEnv, IO, A>.LiftIO(io).As());

    public static Db<DbEnv> Ask()
        => Readable.ask<Db, DbEnv>().As();

    public static Db<Context> Ctx<Context>()
        where Context : DbContext
        => from env in Ask()
            select (Context)env.Context;

    public static Db<Unit> Save() =>
        from env in Ask()
        from _ in liftIO(() => env.Context.SaveChangesAsync())
        select unit;

    public static K<Db, A> Fail<A>(Error error) =>
        Lift(IO.fail<A>(error));

    public static K<Db, A> Catch<A>(K<Db, A> fa, Func<Error, bool> Predicate, Func<Error, K<Db, A>> Fail) =>
        Ask().Bind(env =>
            fa.MapIO(io =>
                io.Catch(
                    Predicate,
                    e => Fail(e).Run(env))));
}

public static class DbExtensions
{
    public static Db<A> As<A>(this K<Db, A> kind)
        => (Db<A>)kind;

    public static K<ReaderT<DbEnv, IO>, A> Run<A>(this K<Db, A> db)
        => db.As().RunDb;

    public static IO<A> Run<A>(this K<Db, A> db, DbEnv env)
        => db.Run().Run(env).As();

    public static Db<A> ToDb<A>(this ValueTask<A> task)
        => Db.Lift(liftIO(async () => await task));

    public static Db<A> ToDb<A>(this Task<A> task)
        => Db.Lift(liftIO(() => task));
}