using System.Collections.Generic;

namespace Shop.Models.Domain.Interface
{
    public interface IItemsRepository
    {
        IEnumerable<Items> GetAllApproved();
        IEnumerable<Items> GetAllUnApproved();
        void UnApproveItem(string slug);
        void ApproveItem(string slug);
        int getNumberofItemsRequests();
        IEnumerable<Items> GetAll();
        IEnumerable<Items> GetTop30(IEnumerable<Items> inputList);
        IEnumerable<Items> GetCouponOfferSlider(IEnumerable<Items> inputList);
        IEnumerable<Items> GetItemsOfferStandard(IEnumerable<Items> inputList);
        IEnumerable<Items> GetItemsOfferStandardAndSlider(IEnumerable<Items> inputList);
        IEnumerable<Items> GetAll(string searchKey, IEnumerable<Items> inputList);
        IEnumerable<Items> GetByLocation(string searchKey, IEnumerable<Items> inputList);
        IEnumerable<Items> GetByName(string searchKey, IEnumerable<Items> inputList);
        IEnumerable<Items> GetByCategory(string searchKey, IEnumerable<Items> inputList);
        IEnumerable<Items> GetByPrice(int searchKey, IEnumerable<Items> inputList);
        IEnumerable<Items> GetItemsNotyetApproved(IEnumerable<Items> inputList);
        IEnumerable<Items> GetItemsApproved(IEnumerable<Items> inputList);
        IEnumerable<Comment> GetCommentsBySlug(string slug);
        IEnumerable<Vote> GetVotesBySlug(string slug);
        Items GetByItemsId(int itemsId);
        Items GetByItemsSlug(string slug);
        void Remove(int itemsId);
        Items GetByItemsIdNotAccepted(int itemsId);
        void Add(Items items);
        void SaveChanges();
        void AddComment(Comment cmt);
        void AddVote(Vote vote);
    }
}
