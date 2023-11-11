using Common.Network.Packets;
using System.Collections.Concurrent;

namespace Common.Network
{
    public class ResultManager
    {
        private readonly ConcurrentDictionary<ID, Action<ResultCodes>> resultDict = new();
        IDPool pool = new();
        public ResultManager() {}
        public Action<ResultCodes>? TryGetAction(int id)
        {
            //ID _id = new(id);
            //resultDict.TryGetValue(_id, out Action<ResultCodes>? action);
            IEnumerable<ID> idArr = resultDict.Select(result => result.Key).Where(__id => __id.GetNumber() == id);
            if(idArr.Any())
                return resultDict[idArr.First()];
            
            return null;
        }
        public ID AddAction(Action<ResultCodes> action) {
            ID id = pool.GetNewID();
            resultDict.TryAdd(id, action);
            return id;
        }
    }
}
